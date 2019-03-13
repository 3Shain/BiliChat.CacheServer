using Microsoft.EntityFrameworkCore;

namespace SanShain.Bilichat.Models
{
    public class BilichatContext:DbContext
    {
        public DbSet<Entry> Entrys { get; set; }

        public BilichatContext(DbContextOptions<BilichatContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntryMap());
        }
    }
}
