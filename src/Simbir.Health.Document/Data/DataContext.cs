using Microsoft.EntityFrameworkCore;
using Simbir.Health.Document.Models;

namespace Simbir.Health.Document.Data
{
    public class DataContext : DbContext
    {
        public DbSet<History> Histories { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>().ToTable("histories").HasKey(i => i.Id);
            modelBuilder.Entity<History>().Property(i => i.Id).UseIdentityColumn();
        }
    }
}
