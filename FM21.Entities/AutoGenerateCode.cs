using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("AutoGenerateCode")]
    public class AutoGenerateCode
    {
        [Key]
        public int CodeID { get; set; }
        public string CodeType { get; set; }
        public long LastCodeValue { get; set; }
    }
}