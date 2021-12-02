using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("BarFormatCodeMaster")]
    public class BarFormatCodeMaster
    {
        [Key]
        public int BarFormatCodeID { get; set; }
        public string BarFormatCode { get; set; }
        public string BarFormatDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
