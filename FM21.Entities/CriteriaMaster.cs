using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("CriteriaMaster")]
    public class CriteriaMaster
    {
        public CriteriaMaster()
        {
            FormulaCriteriaMapping = new HashSet<FormulaCriteriaMapping>();
        }

        [Key]
        public int CriteriaID { get; set; }
        public string CriteriaDescription { get; set; }
        public string ColorCode { get; set; }
        public string CriteriaOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaCriteriaMapping> FormulaCriteriaMapping { get; set; }
    }
}