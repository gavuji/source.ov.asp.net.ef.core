using System;
using System.Text;

namespace FM21.Core.Model
{
    public class FormulaTargetModel
    {
        public string NutrientName { get; set; }
        public int RDIValue { get; set; }
        public int NutrientID { get; set; }
        public int IngredientID { get; set; }

        public decimal NutrientValue { get; set; }
    }
}
