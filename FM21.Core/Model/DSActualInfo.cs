namespace FM21.Core.Model
{
    public class DSActualInfo
    {
        public int NutrientID { get; set; }
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal? PerServingValue { get; set; }
        public decimal? Per100GramValue { get; set; }
        public int DisplayColumnOrder { get; set; }
        public int DisplayItemOrder { get; set; }
        public bool IsAminoAcid { get; set; }
    }
}