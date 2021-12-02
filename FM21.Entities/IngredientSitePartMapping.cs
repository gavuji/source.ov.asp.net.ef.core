using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientSitePartMapping")]
    public class IngredientSitePartMapping
    {
        [Key]
        public int IngredientSitePartMapID { get; set; }
        public int IngredientID { get; set; }
        public int SiteID { get; set; }
        public string PartNumber { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual IngredientMaster Ingredient { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}