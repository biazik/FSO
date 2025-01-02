using System.ComponentModel.DataAnnotations;

namespace FSO.Models
{
    //Tabela rodzajów tagów
    public class Tag
    {
        public int Id { get; set; }
        [MinLength(5, ErrorMessage = "Nazwa musi mieć co najmniej 5 znaków.")]
        [MaxLength(30, ErrorMessage = "Nazwa może mieć maksymalnie 30 znaków.")]
        [Required]
        public string Name { get; set; }
    }
}
