using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientNutrientMapping")]
    public class IngredientNutrientMapping
    {
        [Key]
        public int IngredientNutrientMapID { get; set; }
        public int IngredientID { get; set; }
        public int NutrientID { get; set; }
        public decimal? NutrientValue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual IngredientMaster Ingredient { get; set; }
        public virtual NutrientMaster Nutrient { get; set; }
    }
}