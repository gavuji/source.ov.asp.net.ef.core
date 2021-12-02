using System.ComponentModel.DataAnnotations;

namespace FM21.Core.Model
{
    public class SearchFilter
    {
        public string Search { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {0}")]
        public int PageIndex { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {0}")]
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}