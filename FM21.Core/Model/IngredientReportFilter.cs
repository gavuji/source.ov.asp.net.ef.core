using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FM21.Core.Model
{
    public class IngredientReportFilter
    {
        public string NutrientColumn { get; set; }
        public string RMSatusColumn { get; set; }
        public string IngredientColumn { get; set; }
        public string AllergenColumn { get; set; }
        public string SupplierColumn { get; set; }
        public string UnitMeasurment { get; set; }
        [Required]
        public string siteIDs { get; set; }
        public List<string> DBColumn { get; set; }
        public string IngredientIDs { get; set; }
    }
}