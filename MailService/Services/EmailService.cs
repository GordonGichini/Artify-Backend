using MailService.Data;
using MailService.Models;
using Microsoft.EntityFrameworkCore;

namespace MailService.Services
{
    public class EmailService
    {
        private DbContextOptions<ApplicationDbContext> options;

        public EmailService(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }

        public async Task addDatatoDatabase(EmailLogger logger)
        {
            var _db=new ApplicationDbContext(options);
            _db.EmailLoggers.Add(logger);
            await _db.SaveChangesAsync();
        }
    }
}
