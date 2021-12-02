namespace FM21.Core.Model
{
    public class PDCAASNutrient
    {
        public string NutrientShortName { get; set; }
        public string NutrientName { get; set; }
        public decimal NutrientValue { get; set; }
        public decimal FAOProtein { get; set; }
        public decimal UncorrectedAAScore { get; set; }
        public decimal CorrectedAAScore { get; set; }
    }
}