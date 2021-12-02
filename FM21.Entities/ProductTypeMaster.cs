using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ProductTypeMaster")]
    public class ProductTypeMaster
    {
        public ProductTypeMaster()
        {
            SiteProductTypeMapping = new HashSet<SiteProductTypeMapping>();
            FormulaTypeProductMapping = new HashSet<FormulaTypeProductMapping>();
        }

        [Key]
        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<SiteProductTypeMapping> SiteProductTypeMapping { get; set; }
        public virtual ICollection<FormulaTypeProductMapping> FormulaTypeProductMapping { get; set; }
    }
}