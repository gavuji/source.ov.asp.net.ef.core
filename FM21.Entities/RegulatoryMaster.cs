using System;
using System.ComponentModel.DataAnnotations;

namespace FM21.Entities
{
    public class RegulatoryMaster
    {
        [Key]
        public int RegulatoryId { get; set; }
        public int NutrientId { get; set; }
        public int? OldUsa { get; set; }
        public int? CanadaNi { get; set; }
        public int? CanadaNf { get; set; }
        public int? NewUsRdi { get; set; }
        public int? EU { get; set; }
        public int UnitPerMg { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual NutrientMaster NutrientMaster { get; set; }
    }
}