using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("RMStatusMaster")]
    public class RMStatusMaster
    {
        public RMStatusMaster()
        {
            IngredientMasterGlutenStatus = new HashSet<IngredientMaster>();
            IngredientMasterHalalStatus = new HashSet<IngredientMaster>();
            IngredientMasterOrganicStatus = new HashSet<IngredientMaster>();
            IngredientMasterRegulatoryStatus = new HashSet<IngredientMaster>();
            IngredientMasterSterilization = new HashSet<IngredientMaster>();
        }

        [Key]
        public int RMStatusMasterID { get; set; }
        public string RMStatusType { get; set; }
        public string RMStatus { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<IngredientMaster> IngredientMasterGlutenStatus { get; set; }
        public virtual ICollection<IngredientMaster> IngredientMasterHalalStatus { get; set; }
        public virtual ICollection<IngredientMaster> IngredientMasterOrganicStatus { get; set; }
        public virtual ICollection<IngredientMaster> IngredientMasterRegulatoryStatus { get; set; }
        public virtual ICollection<IngredientMaster> IngredientMasterSterilization { get; set; }
    }
}