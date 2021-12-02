namespace FM21.Core.Model
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CustomerAbbreviation1 { get; set; }
        public string CustomerAbbreviation2 { get; set; }
    }
}