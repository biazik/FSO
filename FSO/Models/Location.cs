using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSO.Models
{
    //Lokalizacje wydarzeń
    public class Location
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        [MinLength(5, ErrorMessage = "Nazwa musi mieć co najmniej 5 znaków.")]
        [MaxLength(50, ErrorMessage = "Nazwa może mieć maksymalnie 50 znaków.")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Szczegóły")]
        [MinLength(5, ErrorMessage = "Szczegóły muszą mieć co najmniej 30 znaków.")]
        [MaxLength(50, ErrorMessage = "Szczegóły mogą mieć maksymalnie 50 znaków.")]
        [Required]
        public string Position { get; set; }
        [Column(TypeName = "decimal(18, 15)")]
        [Range(51.5, 52.5, ErrorMessage = "Podano zbyt odległe wydarzenie.")]
        [Required]
        public decimal lat { get; set; }
        [Column(TypeName = "decimal(18, 15)")]
        [Range(20.5, 21.5, ErrorMessage = "Podano zbyt odległe wydarzenie.")]
        [Required]
        public decimal lon { get; set; }
    }
}
