namespace EAOR.Application.Contracts.Infrastructure.Services
{
    public interface IListenerService
    {
        Task StartListening(CancellationToken cancellationToken);
    }
}
