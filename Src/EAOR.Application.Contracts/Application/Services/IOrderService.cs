using EAOR.Application.Common.Models.Dtos;
using EAOR.Application.Models;

namespace EAOR.Application.Contracts.Application.Services
{
	public interface IOrderService
	{
		Task<List<OrderViewModel>> GetOrders(CancellationToken cancellationToken = default);
		Task AddOrders(List<OrderDto>? order, CancellationToken cancellationToken);
	}
}
