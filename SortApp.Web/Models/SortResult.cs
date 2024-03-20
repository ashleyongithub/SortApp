using System.ComponentModel.DataAnnotations;

namespace SortApp.Web.Models
{
    public class SortResult
    {
        public int Id { get; set; }
        public required string Numbers { get; set; }
        public required string Order { get; set; }
        [Display(Name = "Time Taken (ms)")]
        public string? TimeTaken { get; set; }
    }
}
