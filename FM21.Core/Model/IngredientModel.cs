using System.Collections.Generic;

namespace FM21.Core.Model
{
    public class IngredientModel
    {
        public IngredientMasterModel Ingredient { get; set; }
        public List<IngredientSupplierModel> SupplierInfo { get; set; }
        public List<NutrientModel> NutrientInfo { get; set; } = new List<NutrientModel>();
    }
}