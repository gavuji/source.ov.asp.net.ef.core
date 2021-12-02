namespace FM21.Core.Model
{
    public class NutrientModel
    {
        public int NutrientId { get; set; }
        public string Name { get; set; }
        public int NutrientTypeId { get; set; }
        public bool IsShowOnTarget { get; set; }
        public int UnitOfMeasurementID { get; set; }
        public string MeasurementUnit { get; set; }
        public int IngredientNutrientMapID { get; set; }
        public int IngredientID { get; set; }
        public decimal NutrientValue { get; set; }
    }
}