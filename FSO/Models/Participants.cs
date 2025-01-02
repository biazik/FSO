using Microsoft.AspNetCore.Identity;

namespace FSO.Models
{
    //Tabela dot. stanu obecności tj. interesuje sie czy też nie, potrzebne do statystyk wydarzeń i tagów
    public class Participants
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public virtual Event? Event { get; set; }
        public Guid UserId { get; set; }
        public int Status {  get; set; } // Wybór statusu tj. Uczestniczę, interesuje mnie to, nie interesuje mnie to w wartościach 1-3
    }
}
