using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("BrokerMaster")]
    public class BrokerMaster
    {
        public BrokerMaster()
        {
            IngredientSupplierMapping = new HashSet<IngredientSupplierMapping>();
        }

        [Key]
        public int BrokerID { get; set; }
        public string BrokerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<IngredientSupplierMapping> IngredientSupplierMapping { get; set; }
    }
}