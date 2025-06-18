namespace EAOR.Application.Contracts.Infrastructure.Services
{
	public interface IEmailProcessingService
	{
		Task FetchMail(CancellationToken cancellationToken);
	}
}