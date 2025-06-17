using EAOR.Domain.Common;

namespace EAOR.Domain.Orders
{
    public class Order : Entity
    {
        public string ProductName { get; private set; }

        public int Quantity { get; private set; }

        public decimal Price { get; private set; }

        protected Order(): base()
        {

        }

        public Order(string productName, int quantity, decimal price) : base(Guid.NewGuid())
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
