using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientCategoryMaster")]
    public class IngredientCategoryMaster
    {
        public IngredientCategoryMaster()
        {
            IngredientMaster = new HashSet<IngredientMaster>();
        }

        [Key]
        public int IngredientCategoryID { get; set; }
        public string IngredientCategoryCode { get; set; }
        public string IngredientCategoryDescription { get; set; }
        public string IngredientCategoryGeneralDescription { get; set; }
        public bool IsSubAssemblyCategory { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<IngredientMaster> IngredientMaster { get; set; }
    }
}