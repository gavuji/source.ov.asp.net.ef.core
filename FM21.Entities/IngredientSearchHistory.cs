using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("IngredientSearchHistory")]
    public class IngredientSearchHistory
    {
        [Key]
        public int IngredientSearchID { get; set; }
        public int UserID { get; set; }
        public string SearchData { get; set; }
        public DateTime SearchDate { get; set; }
    }
}