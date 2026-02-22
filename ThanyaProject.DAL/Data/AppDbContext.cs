
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThanyaProject.Models.Model;

namespace ThanyaProject.DAL.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int,IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
      public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Specialtie> Specialties { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<InjuryRecords> InjuryRecords { get; set; }
        public DbSet<EmergancyContact> EmergancyContacts { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany(r => r.UsersRole)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<OrderItem>()
                .HasKey(x => new { x.OrderId, x.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(x => x.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(x => x.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithMany(u => u.MedicalRecords)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InjuryRecords>()
                .HasOne(i => i.User)
                .WithMany(u => u.InjuryRecords)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InjuryRecords>()
                .HasOne(i => i.Doctor)
                .WithMany(d => d.InjuryRecords)
                .HasForeignKey(i => i.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EmergancyContact>()
                .HasOne(e => e.User)
                .WithMany(u => u.EmergencyContacts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Injury)
                .WithMany()
                .HasForeignKey(n => n.InjuryId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Device>()
                .Property(d => d.Lat)
                .HasColumnType("decimal(9,6)");
            modelBuilder.Entity<Device>()
                .Property(d => d.Long)
                .HasColumnType("decimal(9,6)");
            modelBuilder.Entity<Device>()
                .HasOne(d => d.User)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Device>()
                .HasIndex(d => new { d.UserId, d.DeviceId })
                .IsUnique();
        }
    }
}
