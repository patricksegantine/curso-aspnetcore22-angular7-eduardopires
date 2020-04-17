using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Eventos.IO.Infra.Data.Context
{
    public class EventStoreSqlContext : DbContext
    {
        public EventStoreSqlContext(DbContextOptions<EventStoreSqlContext> options) : base(options) { }

        public DbSet<StoredEvent> StoredEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoredEventMap());

            base.OnModelCreating(builder);
        }
    }
}
