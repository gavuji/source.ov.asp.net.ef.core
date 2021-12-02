using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("PowderUnitServingSiteMapping")]
    public class PowderUnitServingSiteMapping
    {
        [Key]
        public int PowderUnitServingSiteMapID { get; set; }
        public int PowderUnitServingID { get; set; }
        public int SiteProductMapID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual UnitServingMaster UnitServingMaster { get; set; }
        public virtual SiteProductTypeMapping SiteProductMap { get; set; }
    }
}