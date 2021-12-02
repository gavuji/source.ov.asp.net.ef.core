using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ReconstitutionMaster")]
    public class ReconstitutionMaster
    {
        public ReconstitutionMaster()
        {
          
        }

        [Key]
        public int ReconstitutionID { get; set; }
        public string ReconstitutionDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}