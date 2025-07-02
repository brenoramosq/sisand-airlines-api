namespace SisandAirlines.Domain.Entities
{
    public class ShoppingCart : Entity
    {
        public Guid? CustomerId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public bool IsFinalized { get; private set; }
        public string Session { get; private set; }


        private readonly List<ShoppingCartItem> _items;
        public IReadOnlyCollection<ShoppingCartItem> Items => _items;


        public ShoppingCart(Guid? customerId, string session)
            :base()
        {
            CustomerId = customerId;
            CreatedDate = DateTime.Now;
            Session = session;

            _items = new List<ShoppingCartItem>();
        }

        public ShoppingCart
        (
            Guid id, 
            Guid? customerId, 
            DateTime createdDate, 
            bool isFinalized, 
            string session
        )
            :base(id)
        {
            CustomerId = customerId;
            CreatedDate = createdDate;
            IsFinalized = isFinalized;
            Session = session;

            _items = new List<ShoppingCartItem>();
        }

        public void AddItem(ShoppingCartItem item)
        {
            _items.Add(item);
        }

        public void Finalize() 
        {
            IsFinalized = true;
        }

        public void SetCustomerId(Guid customerId)
        {
            CustomerId = customerId;
        }

        public bool CanAssociateCustomer() 
        {
            return !IsFinalized && CustomerId is null;
        } 

    }
}
