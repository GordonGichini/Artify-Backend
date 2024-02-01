using MailService.Models;
using Microsoft.EntityFrameworkCore;

namespace MailService.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}
