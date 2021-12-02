using System.Data;

namespace FM21.Core.Model
{
    public class FormulaIngredientAttributes
    {
        public DataTable Attributes { get; set; }
        public string AllergenCode { get; set; }
        public string AllergenName { get; set; }
    }
}