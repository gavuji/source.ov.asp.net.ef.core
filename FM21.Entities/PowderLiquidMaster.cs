using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("PowderLiquidMaster")]
    public class PowderLiquidMaster
    {
        public PowderLiquidMaster()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
        }

        [Key]
        public int PowderLiquidID { get; set; }
        public string LiquidDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
    }
}