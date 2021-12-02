namespace FM21.Core.Model
{
    public class AllergenMasterModel
    {
        public int AllergenID { get; set; }
        public string AllergenCode { get; set; }
        public string AllergenName { get; set; }
        public string AllergenDescription_En { get; set; }
        public string AllergenDescription_Fr { get; set; }
        public string AllergenDescription_Es { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public bool IsUSAAllergen { get; set; }
        public bool IsCANADAAllergen { get; set; }
    }
}