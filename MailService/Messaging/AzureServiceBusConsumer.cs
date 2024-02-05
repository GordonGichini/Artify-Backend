using Azure.Messaging.ServiceBus;
using MailService.Models.Dtos;
using MailService.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MailService.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureServiceBusConsumer> _logger;
        private readonly ServiceBusProcessor _emailProcessor;
        private readonly ServiceBusProcessor _orderProcessor;
        private readonly IMailService _mailService;

        public AzureServiceBusConsumer(IConfiguration configuration, ILogger<AzureServiceBusConsumer> logger, IMailService mailService)
        {
            _configuration = configuration;
            _logger = logger;
            _mailService = mailService;

             string connectionString = _configuration.GetConnectionString("AzureServices:connectionString");
            string queueName = _configuration.GetValue<string>("AzureServices:qName");


            var client = new ServiceBusClient(connectionString);
            _emailProcessor = client.CreateProcessor(queueName);
            _orderProcessor = client.CreateProcessor("orderplaced");
        }

        public async Task Start()
        {
            _emailProcessor.ProcessMessageAsync += OnRegisterUser;
            _emailProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailProcessor.StartProcessingAsync();

            _orderProcessor.ProcessMessageAsync += OnOrder;
            _orderProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailProcessor.StopProcessingAsync();
            await _emailProcessor.DisposeAsync();

            await _orderProcessor.StopProcessingAsync();
            await _orderProcessor.DisposeAsync();
        }

        private async Task OnOrder(ProcessMessageEventArgs args)
        {
            try
            {
                var message = args.Message;
                var body = Encoding.UTF8.GetString(message.Body);
                var user = JsonConvert.DeserializeObject<UserMessageDto>(body);

                StringBuilder emailContent = new StringBuilder();
                emailContent.Append("<img src=\"https://cdn.pixabay.com/photo/2017/11/08/22/48/gallery-2931925_1280.jpg\" width=\"1000\" height=\"600\">");
                emailContent.Append($"<h1> Hello {user.Name}</h1>");
                emailContent.AppendLine("<br/> Your order was placed successfully.");
                emailContent.AppendLine("<p>You can make another order!!</p>");

                var emailDto = new UserMessageDto
                {
                    Email = user.Email,
                    Name = user.Name,
                    Message = emailContent.ToString()
                };
                await _mailService.SendEmail(emailDto);

                // Log successful message processing
                _logger.LogInformation("Order confirmation email sent to {Email}", user.Email);

                // Mark the message as completed to remove it from the queue
                await args.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order message: {ErrorMessage}", ex.Message);

                // Send an email notification to admin
                var adminNotification = new UserMessageDto
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    Message = $"Error processing order message: {ex.Message}"
                };
                await _mailService.SendEmail(adminNotification);

                // Mark the message as completed to remove it from the queue
                await args.CompleteMessageAsync(args.Message);
            }
        }

        private async Task OnRegisterUser(ProcessMessageEventArgs args)
        {
            try
            {
                var message = args.Message;
                var body = Encoding.UTF8.GetString(message.Body);
                var user = JsonConvert.DeserializeObject<UserMessageDto>(body);

                StringBuilder emailContent = new StringBuilder();
                emailContent.Append("<img src=\"https://cdn.pixabay.com/photo/2017/11/08/22/48/gallery-2931925_1280.jpg\" width=\"800\" height=\"500\">");
                emailContent.Append($"<h1> Hello {user.Name}</h1>");
                emailContent.AppendLine("<br/> Welcome to ArtifyAuction");
                emailContent.AppendLine("<p>Get Amazing Artifacts on Auction!</p>");

                var emailDto = new UserMessageDto
                {
                    Email = user.Email,
                    Name = user.Name,
                    Message = emailContent.ToString()
                };
                await _mailService.SendEmail(emailDto);

                // Log successful message processing
                _logger.LogInformation("Welcome email sent to {Email}", user.Email);

                // Mark the message as completed to remove it from the queue
                await args.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing register user message: {ErrorMessage}", ex.Message);

                // Send an email notification to admin
                var adminNotification = new UserMessageDto
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    Message = $"Error processing register user message: {ex.Message}"
                };
                await _mailService.SendEmail(adminNotification);

                // Mark the message as completed to remove it from the queue
                await args.CompleteMessageAsync(args.Message);
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Log error
            _logger.LogError(args.Exception, "An error occurred while processing message: {ErrorMessage}", args.Exception.Message);

            // Send email notification to admin
            var adminNotification = new UserMessageDto
            {
                Name = "Admin",
                Email = "admin@example.com",
                Message = $"An error occurred while processing message: {args.Exception.Message}"
            };
            return _mailService.SendEmail(adminNotification);
        }
    }
}
