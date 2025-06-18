namespace EAOR.Application.Contracts.Application.Services
{
    public interface IEmailService
    {
        Task AddEmail(string body, byte[] rawData, DateTime receivedDate, CancellationToken cancellationToken);
    }
}
