using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("AllergenMaster")]
    public class AllergenMaster
    {
        public AllergenMaster()
        {
            IngredientAllergenMapping = new HashSet<IngredientAllergenMapping>();
        }

        [Key]
        public int AllergenID { get; set; }
        public string AllergenCode { get; set; }
        public string AllergenName { get; set; }
        public string AllergenDescription_En { get; set; }
        public string AllergenDescription_Fr { get; set; }
        public string AllergenDescription_Es { get; set; }
        public bool? IsDeleted { get; set; }
        public bool?  IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsUSAAllergen { get; set; }
        public bool IsCANADAAllergen { get; set; }

        public virtual ICollection<IngredientAllergenMapping> IngredientAllergenMapping { get; set; }
    }
}