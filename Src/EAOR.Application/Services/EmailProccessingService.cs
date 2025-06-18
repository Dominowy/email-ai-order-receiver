using EAOR.Application.Common.Models.Identifier;
using EAOR.Application.Contracts.Application.Services;
using EAOR.Application.Contracts.Infrastructure.Context;
using EAOR.Application.Contracts.Infrastructure.Services;
using EAOR.Domain.Emails;

namespace EAOR.Application.Services
{
    public class EmailProccessingService : IEmailProccessingService
	{
        private readonly IApplicationDbContext _dbContext;
		private readonly IEmailFetchService _emailFetchService;

		public EmailProccessingService(IApplicationDbContext dbContext, IEmailFetchService emailFetchService)
        {
            _dbContext = dbContext;
            _emailFetchService = emailFetchService;

		}

		public async Task<List<Identifier>> GetAllUnseenEmails(CancellationToken cancellationToken)
        {
            var unseenEmails = await _emailFetchService.FetchUnseenMail(cancellationToken);

            return unseenEmails;
		}

		public async Task<Email> AddUnseenEmail(Identifier unseenEmail, CancellationToken cancellationToken)
		{
			var fetchedEmail = await _emailFetchService.FetchMail(unseenEmail, cancellationToken);

			var email = new Email(fetchedEmail.Body, fetchedEmail.ReceivedDate);

			await _dbContext.Set<Email>().AddAsync(email, cancellationToken);

			var emlFile = new EmailFile(fetchedEmail.Data, email.Id);

			await _dbContext.Set<EmailFile>().AddAsync(emlFile, cancellationToken);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return email;
		}

		public async Task MarkedMailToSeen(Identifier unseenEmail, CancellationToken cancellationToken)
		{
			await _emailFetchService.MarkedSeenEmail(unseenEmail, cancellationToken);
		}
	}
}
