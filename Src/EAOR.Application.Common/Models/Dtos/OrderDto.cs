namespace EAOR.Application.Common.Models.Dtos
{
	public class OrderDto
	{
		public string ProductName { get; set; }

		public int Quantity { get; set; }

		public decimal Price { get; set; }

		public string Currency { get; set; }
	}
}
