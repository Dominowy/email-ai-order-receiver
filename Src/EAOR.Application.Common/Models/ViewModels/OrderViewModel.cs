using EAOR.Domain.Orders;

namespace EAOR.Application.Models
{
	public class OrderViewModel
	{
		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public static OrderViewModel Map(Order order)
		{
			return new OrderViewModel
			{
				ProductName = order.ProductName,
				Quantity = order.Quantity,
				Price = order.Price
			};
		}
	}
}
