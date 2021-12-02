using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("KosherCodeMaster")]
    public class KosherCodeMaster
    {
        public KosherCodeMaster()
        {
            IngredientSupplierMapping = new HashSet<IngredientSupplierMapping>();
        }

        [Key]
        public int KosherCodeID { get; set; }
        public string KosherCode { get; set; }
        public string KosherCodeDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<IngredientSupplierMapping> IngredientSupplierMapping { get; set; }
    }
}