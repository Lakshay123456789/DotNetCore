using System.ComponentModel.DataAnnotations;

namespace TestCore.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? MovieName { get; set; }
        [Required]
        public DateTime MovieDate { get; set; }
        [Required]
        public string? MovieGenre { get; set; }
        [Required]
        public string? MovieCast { get; set; }
    }
}
