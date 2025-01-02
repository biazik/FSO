    using Microsoft.AspNetCore.Identity;

namespace FSO.Models
{
    //Tabela do tagowania wydarzeń
    public class EventTag
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public virtual Event? Event { get; set; }
        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
