using EAOR.Application.Contracts.Infrastructure.Context;
using EAOR.Domain.Emails;
using EAOR.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace EAOR.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Email> Email { get; set; }
        public DbSet<EmailFile> EmailFile { get; set; }
        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmailFile>()
                .HasOne(m => m.Email)
                .WithOne(m => m.EmailFile)
                .HasForeignKey<EmailFile>(m => m.EmailId)
                .OnDelete(DeleteBehavior.Restrict);;
        }
    }
}
