using EAOR.Domain.Common;

namespace EAOR.Domain.Orders
{
    public class Order : BaseEntity
    {
        public string ProductName { get; private set; }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        protected Order(): base()
        {

        }

        public Order(Guid id, string productName, int quantity, decimal price) : base(id)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
