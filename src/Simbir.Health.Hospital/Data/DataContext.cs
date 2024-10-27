using Microsoft.EntityFrameworkCore;
using Simbir.Health.Hospital.Models;

namespace Simbir.Health.Hospital.Data
{
    public class DataContext : DbContext
    {
        public DbSet<HospitalEntity> Hospitals { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HospitalEntity>().ToTable("hospitals").HasKey(i => i.Id);
            modelBuilder.Entity<HospitalEntity>().Property(i => i.Id).UseIdentityColumn();
            modelBuilder.Entity<HospitalEntity>().Property(i => i.Name).HasColumnType("varchar(256)");
            modelBuilder.Entity<HospitalEntity>().Property(i => i.Address).HasColumnType("varchar(256)");
            modelBuilder.Entity<HospitalEntity>().Property(i => i.ContactPhone).HasColumnType("varchar(256)");

            modelBuilder.Entity<Room>().ToTable("rooms").HasKey(i => i.Id);
            modelBuilder.Entity<Room>().Property(i => i.Id).UseIdentityColumn();
            modelBuilder.Entity<Room>().Property(i => i.Name).HasColumnType("varchar(256)");
            modelBuilder.Entity<Room>().HasOne<HospitalEntity>().WithMany().HasForeignKey(r => r.HospitalId);
            modelBuilder.Entity<Room>().HasIndex(i => new { i.Name, i.HospitalId }).IsUnique();
        }
    }
}
