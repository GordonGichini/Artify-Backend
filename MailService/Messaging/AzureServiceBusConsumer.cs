using Azure.Messaging.ServiceBus;
using MailService.Models;
using MailService.Models.Dtos;
using MailService.Service;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace MailService.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _qName;
        private readonly ServiceBusProcessor _emailProcessor;
        private readonly ServiceBusProcessor _orderProcessor;
        private readonly MailsService _emailService;
        private readonly EmailService _email;


        public AzureServiceBusConsumer(IConfiguration configuration,EmailService service)
        {
            _configuration = configuration;
            _email= service;

            _connectionString = _configuration.GetValue<string>("AzureServices");
            _qName= _configuration.GetValue<string>("qName:artifyauction");

            var client = new ServiceBusClient(_connectionString);
            _emailProcessor = client.CreateProcessor(_qName);
            _orderProcessor = client.CreateProcessor("orderplaced");
            _emailService = new MailsService(configuration);

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
        private async Task OnOrder(ProcessMessageEventArgs arg)
        {

            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);//read  as String
            var users = JsonConvert.DeserializeObject<UserMessageDto>(body);//string to UserMessageDto

            try
            {

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<img src=\"https://cdn.pixabay.com/photo/2017/11/08/22/48/gallery-2931925_1280.jpg\" width=\"1000\" height=\"600\">");
                stringBuilder.Append("<h1> Hello " + users.Name + "</h1>");
                stringBuilder.AppendLine("<br/> Your Order was placed Successfully ");

                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');
                stringBuilder.Append("<p>You can Make another order!!</p>");

                var user = new UserMessageDto()
                {
                    Email = users.Email,
                    Name = users.Name,
                };
                await _emailService.sendEmail(user, stringBuilder.ToString());


                //insert  to Database
                var emaiLLogger = new EmailLogger()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Message = stringBuilder.ToString(),
                    DateTime = DateTime.Now,

                };
                await _email.addDatatoDatabase(emaiLLogger);

                await arg.CompleteMessageAsync(arg.Message);//we are done delete the message from the queue 
            }
            catch (Exception ex)
            {
                throw;
                //send an Email to Admin
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            //send Email to Admin 
             return Task.CompletedTask;
        }

        private async Task OnRegisterUser(ProcessMessageEventArgs arg)
        {
           
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);//read  as String
            var user = JsonConvert.DeserializeObject<UserMessageDto>(body);//string to UserMessageDto

            try
            {

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<img src=\"https://cdn.pixabay.com/photo/2017/11/08/22/48/gallery-2931925_1280.jpg\" width=\"800\" height=\"500\">");
                stringBuilder.Append("<h1> Hello " + user.Name + "</h1>");
                stringBuilder.AppendLine("<br/>Welcome to ArtBbase App");
                stringBuilder.Append("<br/>");
                stringBuilder.Append('\n');
                stringBuilder.Append("<p>Bid, Win and Get your favorite art Gallery here!!</p>");
                await _emailService.sendEmail(user, stringBuilder.ToString());


                //insert  to Database
                var emaiLLogger = new EmailLogger()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Message = stringBuilder.ToString(),
                    DateTime = DateTime.Now,

                };
                await _email.addDatatoDatabase(emaiLLogger);

                await arg.CompleteMessageAsync(arg.Message);//we are done delete the message from the queue 
            }catch (Exception ex)
            {
                throw;
                //send an Email to Admin
            }
        }

       
    }
}

