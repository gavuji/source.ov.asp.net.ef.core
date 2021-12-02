using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("NutrientTypeMaster")]
    public class NutrientTypeMaster
    {
        public NutrientTypeMaster()
        {
            NutrientMaster = new HashSet<NutrientMaster>();
        }

        [Key]
        public int NutrientTypeID { get; set; }
        public string TypeName { get; set; }    
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<NutrientMaster> NutrientMaster { get; set; }
    }
}