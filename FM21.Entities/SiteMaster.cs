using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("SiteMaster")]
    public class SiteMaster
    {
        public SiteMaster()
        {
            IngredientSitePartMapping = new HashSet<IngredientSitePartMapping>();
            IngredientSupplierMapping = new HashSet<IngredientSupplierMapping>();
            PowderBlenderSiteMapping = new HashSet<PowderBlenderSiteMapping>();
            SiteInstructionCategoryMapping = new HashSet<SiteInstructionCategoryMapping>();
            SiteProductTypeMapping = new HashSet<SiteProductTypeMapping>();
            SiteProductionLineMapping = new HashSet<SiteProductionLineMapping>();
            FormulaChangeCode = new HashSet<FormulaChangeCode>();
            SiteProcessCode = new HashSet<SiteProcessCode>();
            SitterWidthMaster = new HashSet<SitterWidthMaster>();
            FormulaRossCode = new HashSet<FormulaRossCode>();
        }

        [Key]
        public int SiteID { get; set; }
        public string SiteCode { get; set; }
        public string SiteDescription { get; set; }
        public string S30CodePrefix { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<IngredientSitePartMapping> IngredientSitePartMapping { get; set; }
        public virtual ICollection<IngredientSupplierMapping> IngredientSupplierMapping { get; set; }
        public virtual ICollection<PowderBlenderSiteMapping> PowderBlenderSiteMapping { get; set; }
        public virtual ICollection<SiteInstructionCategoryMapping> SiteInstructionCategoryMapping { get; set; }
        public virtual ICollection<SiteProductTypeMapping> SiteProductTypeMapping { get; set; }
        public virtual ICollection<SiteProductionLineMapping> SiteProductionLineMapping { get; set; }
        public virtual ICollection<FormulaChangeCode> FormulaChangeCode { get; set; }
        public virtual ICollection<SiteProcessCode> SiteProcessCode { get; set; }
        public virtual ICollection<SitterWidthMaster> SitterWidthMaster { get; set; }
        public virtual ICollection<FormulaRossCode> FormulaRossCode { get; set; }
    }
}