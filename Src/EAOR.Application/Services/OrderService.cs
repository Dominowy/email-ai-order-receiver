using EAOR.Application.Contracts.Infrastructure.Context;
using EAOR.Application.Contracts.Services;
using EAOR.Application.Models;
using EAOR.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace EAOR.Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IApplicationDbContext _dbContext;

		public OrderService(IApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<OrderViewModel>> GetOrders(CancellationToken cancellationToken)
		{
			var orders = await _dbContext.Set<Order>()
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return [.. orders.Select(OrderViewModel.Map)];
		}
	}
}
