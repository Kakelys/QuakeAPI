using System.ComponentModel.DataAnnotations;

namespace QuakeAPI.DTO
{
    public class Page
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; }
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; }
    }
}