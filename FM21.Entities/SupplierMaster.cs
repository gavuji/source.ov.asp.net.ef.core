using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("SupplierMaster")]
    public class SupplierMaster
    {
        public SupplierMaster()
        {
            IngredientSupplierMapping = new HashSet<IngredientSupplierMapping>();
        }

        [Key]
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string SupplierAbbreviation1 { get; set; }
        public string SupplierAbbreviation2 { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<IngredientSupplierMapping> IngredientSupplierMapping { get; set; }
    }
}