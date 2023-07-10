using System.ComponentModel.DataAnnotations;

namespace QuakeAPI.DTO.Analytic
{
    public class MostPopularLocation
    {
        [Range(1, 12)]
        public int Month { get;set; }
        [Range(1, 9999)]
        public int Year { get;set; }
    }
}