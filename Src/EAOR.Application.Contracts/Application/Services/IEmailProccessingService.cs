using EAOR.Application.Common.Models.Identifier;
using EAOR.Domain.Emails;

namespace EAOR.Application.Contracts.Application.Services
{
    public interface IEmailProccessingService
	{
		Task<List<Identifier>> GetAllUnseenEmails(CancellationToken cancellationToken);

		Task<Email> AddUnseenEmail(Identifier unseenEmail, CancellationToken cancellationToken);
		Task MarkedMailToSeen(Identifier unseenEmail, CancellationToken cancellationToken);
	}
}
