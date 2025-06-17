using EAOR.Domain.Orders;

namespace EAOR.Application.Models
{
	public class OrderDto
	{
		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public static OrderDto Map(Order order)
		{
			return new OrderDto
			{
				ProductName = order.ProductName,
				Quantity = order.Quantity,
				Price = order.Price
			};
		}
	}
}
