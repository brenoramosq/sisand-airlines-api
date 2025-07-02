using SisandAirlines.Domain.Enums;

namespace SisandAirlines.Domain.Entities
{
    public class Purchase : Entity
    {
        public Guid CustomerId { get; private set; }
        public Guid ShoppingCartId { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public string ConfirmationCode { get; private set; }

        public Purchase(Guid customerId, Guid shoppingCartId, decimal totalAmount)
            :base()
        {
            CustomerId = customerId;
            ShoppingCartId = shoppingCartId;
            PurchaseDate = DateTime.Now;
            TotalAmount = totalAmount;
            PaymentMethod = PaymentMethod.Pix;
            ConfirmationCode = GenerateConfirmationCode();
        }

        public Purchase(Guid id, Guid customerId, Guid shoppingCartId, decimal totalAmount, DateTime purchaseDate, PaymentMethod paymentMethod, string confirmationCode)
            : base(id)
        {
            CustomerId = customerId;
            ShoppingCartId = shoppingCartId;
            PurchaseDate = purchaseDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            ConfirmationCode = confirmationCode;
        }

        private string GenerateConfirmationCode()
        {
            return $"SISAND-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
