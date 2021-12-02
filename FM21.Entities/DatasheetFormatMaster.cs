using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("DatasheetFormatMaster")]
    public class DatasheetFormatMaster
    {
        public DatasheetFormatMaster()
        {
            FormulaDatasheetMapping = new HashSet<FormulaDatasheetMapping>();
            FormulaMaster = new HashSet<FormulaMaster>();
        }

        [Key]
        public int DatasheetFormatID { get; set; }
        public string DatasheetCode { get; set; }
        public string DatasheetDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaDatasheetMapping> FormulaDatasheetMapping { get; set; }
        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
    }
}