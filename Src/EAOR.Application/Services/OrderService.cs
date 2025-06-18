using EAOR.Application.Common.Models.Dtos;
using EAOR.Application.Contracts.Application.Services;
using EAOR.Application.Contracts.Infrastructure.Context;
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

		public async Task<List<OrderViewModel>> GetOrders(CancellationToken cancellationToken = default)
		{
			var orders = await _dbContext.Set<Order>()
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return [.. orders.Select(OrderViewModel.Map)];
		}

		public async Task AddOrders(List<OrderDto> order, CancellationToken cancellationToken)
		{
			if (order == null || !order.Any()) return;

			var orders = order.Select(m => new Order(m.ProductName, m.Quantity, m.Price)).ToList();

			_dbContext.Set<Order>().AddRange(orders);

			await _dbContext.SaveChangesAsync(cancellationToken);
		}

	}
}
