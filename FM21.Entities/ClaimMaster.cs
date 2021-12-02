using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ClaimMaster")]
    public class ClaimMaster
    {
        public ClaimMaster()
        {
            FormulaClaimMapping = new HashSet<FormulaClaimMapping>();
        }

        [Key]
        public int ClaimID { get; set; }
        public string ClaimCode { get; set; }
        public string ClaimDescription { get; set; }
        public string ClaimGroupType { get; set; }
        public bool HasImpact { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaClaimMapping> FormulaClaimMapping { get; set; }
    }
}