using Microsoft.EntityFrameworkCore;
using Simbir.Health.Timetable.Models;

namespace Simbir.Health.Timetable.Data
{
    public class DataContext : DbContext
    {
        public DbSet<TimeTableEntity> TimeTable { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeTableEntity>().ToTable("time_table").HasKey(i => i.Id);
            modelBuilder.Entity<TimeTableEntity>().Property(i => i.Id).UseIdentityColumn();
            modelBuilder.Entity<TimeTableEntity>().HasIndex(i => new { i.HospitalId, i.DoctorId, i.RoomId, i.StartTime, i.EndTime }).IsUnique();

            modelBuilder.Entity<Appointment>().ToTable("appointments").HasKey(i => i.Id);
            modelBuilder.Entity<Appointment>().Property(i => i.Id).UseIdentityColumn();
            modelBuilder.Entity<Appointment>().HasOne<TimeTableEntity>().WithMany().HasForeignKey(r => r.TimetableId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Appointment>().HasIndex(i => new { i.TimetableId, i.ReceptionTime }).IsUnique();
        }
    }
}
