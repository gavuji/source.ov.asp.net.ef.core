using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("NutrientMaster")]
    public class NutrientMaster
    {
        public NutrientMaster()
        {
            FormulaDatasheetMapping = new HashSet<FormulaDatasheetMapping>();
            IngredientNutrientMapping = new HashSet<IngredientNutrientMapping>();
            RegulatoryMaster = new HashSet<RegulatoryMaster>();
        }

        [Key]
        public int NutrientID { get; set; }
        public string Name { get; set; }
        public int NutrientTypeID { get; set; }
        public int UnitOfMeasurementID { get; set; }
        public decimal? DefaultValue { get; set; }
        public bool IsShowOnTarget { get; set; }
        public int DisplayColumnOrder { get; set; }
        public int DisplayItemOrder { get; set; }
        public bool IsActiveNutrient { get; set; }
        public bool IsAminoAcid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual NutrientTypeMaster NutrientTypeMaster { get; set; }
        public virtual UnitOfMeasurementMaster UnitOfMeasurement { get; set; }
        public virtual ICollection<FormulaDatasheetMapping> FormulaDatasheetMapping { get; set; }
        public virtual ICollection<IngredientNutrientMapping> IngredientNutrientMapping { get; set; }
        public virtual ICollection<RegulatoryMaster> RegulatoryMaster { get; set; }
    }
}