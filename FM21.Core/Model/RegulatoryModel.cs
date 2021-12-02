namespace FM21.Core.Model
{

    public class RegulatoryModel
    {
        public int RegulatoryId { get; set; }
        public string Nutrient { get; set; }
        public int NutrientId { get; set; }
        public int? OldUsa { get; set; }
        public int? CanadaNi { get; set; }
        public int? CanadaNf { get; set; }
        public int? NewUsRdi { get; set; }
        public int? EU { get; set; }
        public string Unit { get; set; }
        public int UnitPerMg { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}