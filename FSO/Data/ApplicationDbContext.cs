using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FSO.Models;

namespace FSO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FSO.Models.Event> Event { get; set; } = default!;
        public DbSet<FSO.Models.Tag> Tag { get; set; } = default!;
        public DbSet<FSO.Models.Location> Location { get; set; } = default!;
        public DbSet<FSO.Models.EventType> EventType { get; set; } = default!;
        public DbSet<FSO.Models.EventTag> EventTag { get; set; } = default!;
        public DbSet<FSO.Models.Participants> Participants { get; set; } = default!;
        public DbSet<FSO.Models.EventView> EventView { get; set; } = default!;
    }
}
