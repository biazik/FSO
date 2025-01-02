using FSO.Attributes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace FSO.Models
{
    //Tabela wydarzeń
    // Jednak użyty zostanie viewmodel, żeby dodać obsługę tagów.
    [TimeDifference(ErrorMessage = "Daty są nieprawidłowe.")]
    public class Event
    {
        public int Id { get; set; }
        [Display(Name="Tytuł")]
        [MinLength(5, ErrorMessage = "Tytuł musi mieć co najmniej 5 znaków.")]
        [MaxLength(50, ErrorMessage = "Tytuł może mieć maksymalnie 50 znaków.")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [MinLength(30, ErrorMessage = "Opis musi mieć co najmniej 30 znaków.")]
        [MaxLength(250, ErrorMessage = "Opis może mieć maksymalnie 250 znaków.")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Typ wydarzenia")]
        public bool isPublic { get; set; }
        public int EventTypeId { get; set; }
        [Display(Name = "Rodzaj")]
        public virtual EventType? EventType { get; set; }
        public int LocationId { get; set; }
        [Display(Name = "Lokalizacja")]
        public virtual Location? Location { get; set; }
        [Display(Name = "Początek wydarzenia")]
        public DateTime Start {  get; set; }
        [Display(Name = "Koniec wydarzenia")]
        [Required]
        public DateTime End { get; set; }
        public Guid CreatorId { get; set; }
    }
}
