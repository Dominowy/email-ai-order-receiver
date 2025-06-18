using EAOR.Application.Common.Models.Identifier;
using EAOR.Application.Common.Models.Message;

namespace EAOR.Application.Contracts.Infrastructure.Services
{
	public interface IEmailFetchService
	{
		Task<List<Identifier>> FetchUnseenMail(CancellationToken cancellationToken);
		Task<EmailMessage> FetchMail(Identifier uid, CancellationToken cancellationToken);
		Task MarkedSeenEmail(Identifier identifier, CancellationToken cancellationToken);
	}
}