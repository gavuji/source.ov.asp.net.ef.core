namespace FM21.Core.Model
{
    public class IngredientListInfo
    {
        public int IngredientID { get; set; }
        public string PartNumber { get; set; }
        public string IngredientDescription { get; set; }
        public int IngredientCategoryID { get; set; }
        public string IngredientName { get; set; }
        public decimal IngUsedPercentInFormula { get; set; }
        public decimal IngUsedPercentInIngredient { get; set; }
        public decimal GramPerServe { get; set; }
        public int OrderNumber { get; set; }
    }
}