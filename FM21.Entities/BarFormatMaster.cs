using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("BarFormatMaster")]
    public class BarFormatMaster
    {
        [Key]
        public int BarFormatID { get; set; }
        public string BarFormatCode { get; set; }
        public string BarFormatType { get; set; }
        public string BarFormatDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int DisplayOrder { get; set; }
    }
}