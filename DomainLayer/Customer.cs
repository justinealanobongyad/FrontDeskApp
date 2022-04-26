namespace DomainLayer
{
    public class Customer
    {
        public int customerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }

        public Customer()
        {
            customerId = 0;
            firstName = String.Empty;
            lastName = String.Empty;
            phoneNumber = String.Empty;
        }
    }
}