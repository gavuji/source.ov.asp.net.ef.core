using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientSupplierMapping")]
    public class IngredientSupplierMapping
    {
        [Key]
        public int IngredientSupplierID { get; set; }
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
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual BrokerMaster Broker { get; set; }
        public virtual IngredientMaster Ingredient { get; set; }
        public virtual KosherCodeMaster KosherCode { get; set; }
        public virtual SupplierMaster Supplier { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}