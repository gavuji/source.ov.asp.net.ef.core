using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FM21.Entities
{
 
    [Table("FormulaSearchHistory")]
    public class FormulaSearchHistory
    {
        [Key]
        public int FormulaSearchID { get; set; }
        public int UserID { get; set; }
        public string SearchData { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
