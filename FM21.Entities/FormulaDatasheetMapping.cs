using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaDatasheetMapping")]
    public class FormulaDatasheetMapping
    {
        [Key]
        public int FormulaDatasheetMapID { get; set; }
        public int FormulaID { get; set; }
        public int? DatasheetFormatID { get; set; }
        public int? NutrientID { get; set; }
        public decimal? Target { get; set; }
        public decimal? Override { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual DatasheetFormatMaster DatasheetFormat { get; set; }
        public virtual FormulaMaster Formula { get; set; }
        public virtual NutrientMaster Nutrient { get; set; }
    }
}