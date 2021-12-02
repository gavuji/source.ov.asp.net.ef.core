using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("UnitOfMeasurementMaster")]
    public class UnitOfMeasurementMaster
    {
        public UnitOfMeasurementMaster()
        {
            NutrientMaster = new HashSet<NutrientMaster>();
            IngredientMaster = new HashSet<IngredientMaster>();
        }

        [Key]
        public int UnitOfMeasurementID { get; set; }
        public string MeasurementUnit { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<NutrientMaster> NutrientMaster { get; set; }
        public virtual ICollection<IngredientMaster> IngredientMaster { get; set; }
    }
}