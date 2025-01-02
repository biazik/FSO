using System.ComponentModel.DataAnnotations;

namespace FSO.Models
{
    //Model do tworzenia wydarzenia
    public class EventView
    {
        public int Id { get; set; }
        public Event Event { get; set; } = new Event();
        public List<Tag> AvailableTags { get; set; } = new List<Tag>();
        [Required(ErrorMessage = "Musisz wybrać przynajmniej jeden tag.")]
        [MinLength(1, ErrorMessage = "Musisz wybrać przynajmniej jeden tag.")]
        public List<int> SelectedTagIds { get; set; } = new List<int>();
    }
}
