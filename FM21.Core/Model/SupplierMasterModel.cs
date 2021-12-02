namespace FM21.Core.Model
{
    public class SupplierMasterModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string SupplierAbbreviation1 { get; set; }
        public string SupplierAbbreviation2 { get; set; }
    }
}