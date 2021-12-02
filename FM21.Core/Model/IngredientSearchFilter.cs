using System.ComponentModel.DataAnnotations;

namespace FM21.Core.Model
{
    public class IngredientSearchFilter
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {0}")]
        public int PageIndex { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {0}")]
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int SiteID { get; set; }
        public string DisplayColumn { get; set; }
        public string SearchField1 { get; set; }
        public string SearchText1 { get; set; }
        public string SearchCondition1 { get; set; }
        public string SearchField2 { get; set; }
        public string SearchText2 { get; set; }
        public string SearchCondition2 { get; set; }
        public string SearchField3 { get; set; }
        public string SearchText3 { get; set; }
        public string SearchCondition3 { get; set; }
        public string NutrientColumn { get; set; }
        public string RMSatusColumn { get; set; }
        public string IngredientColumn { get; set; }
        public string AllergenColumn { get; set; }
        public string SupplierColumn { get; set; }
        public string UnitMeasurment { get; set; }
    }
}