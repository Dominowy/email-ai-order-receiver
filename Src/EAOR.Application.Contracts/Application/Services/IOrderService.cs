using EAOR.Application.Models;

namespace EAOR.Application.Contracts.Services
{
	public interface IOrderService
	{
		Task<List<OrderViewModel>> GetOrders(CancellationToken cancellationToken);
	}
}
