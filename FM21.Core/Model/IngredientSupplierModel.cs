using System;

namespace FM21.Core.Model
{
    public class IngredientSupplierModel
    {
        public int? IngredientSupplierID { get; set; }
        public int? IngredientID { get; set; }
        public int? BrokerID { get; set; }
        public int? ManufactureID { get; set; }
        public int? SiteID { get; set; }
        public int? KosherCodeID { get; set; }
        public string BrokerDetail { get; set; }
        public string ManufactureDetail { get; set; }
        public string BrokerDescription { get; set; }
        public string ManufactureDescription { get; set; }
        public string ManufactureLocation { get; set; }
        public string KosherAgency { get; set; }
        public decimal? Price { get; set; }
        public DateTime? QuotedDate { get; set; }
        public DateTime? KosherExpireDate { get; set; }
    }
}