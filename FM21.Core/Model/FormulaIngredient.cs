namespace FM21.Core.Model
{
    public class FormulaIngredient
    {
        public int IngredientID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Percent { get; set; }
    }
    public class FormulaTargetInfoParam
    {
        public string IngredientID { get; set; }
        public int DatasheetFormatID { get; set; }
    }
}