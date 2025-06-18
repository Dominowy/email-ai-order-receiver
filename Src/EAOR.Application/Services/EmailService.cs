using EAOR.Application.Contracts.Application.Services;
using EAOR.Application.Contracts.Infrastructure.Context;
using EAOR.Domain.Emails;

namespace EAOR.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IApplicationDbContext _dbContext;

        public EmailService(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEmail(string body, byte[] rawData, DateTime receivedDate, CancellationToken cancellationToken)
        {
            var email = new Email(body, receivedDate);

            await _dbContext.Set<Email>().AddAsync(email, cancellationToken);

            var emlFile = new EmailFile(rawData, email.Id);

            await _dbContext.Set<EmailFile>().AddAsync(emlFile, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
