using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ProjectMaster")]
    public class ProjectMaster
    {
        public ProjectMaster()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
        }

        [Key]
        public int ProjectId { get; set; }
        public int ProjectCode { get; set; }
        public string NPICode { get; set; }
        public string ProjectDescription { get; set; }
        public int CustomerId { get; set; }       
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Customer CustomerMaster { get; set; }
        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
    }
}