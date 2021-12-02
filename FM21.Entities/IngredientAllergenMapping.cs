using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientAllergenMapping")]
    public class IngredientAllergenMapping
    {
        [Key]
        public int IngredientAllergenMapID { get; set; }
        public int IngredientID { get; set; }
        public int AllergenID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual AllergenMaster Allergen { get; set; }
        public virtual IngredientMaster Ingredient { get; set; }
    }
}