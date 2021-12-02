using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("UnitServingMaster")]
    public class UnitServingMaster
    {
        public UnitServingMaster()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
            PowderUnitServingSiteMapping = new HashSet<PowderUnitServingSiteMapping>();
        }

        [Key]
        public int UnitServingID { get; set; }
        public string UnitServingType { get; set; }
        public string UnitDescription { get; set; }
        public int ProductTypeID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
        public virtual ICollection<PowderUnitServingSiteMapping> PowderUnitServingSiteMapping { get; set; }
    }
}