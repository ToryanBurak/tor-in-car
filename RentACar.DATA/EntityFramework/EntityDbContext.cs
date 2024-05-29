using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RentACar.DATA.EntityFramework
{
    public partial class EntityDbContext : DbContext
    {
        public EntityDbContext()
            : base("name=EntityDbContext")
        {
        }

        public virtual DbSet<Advert> Advert { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CarMark> CarMark { get; set; }
        public virtual DbSet<CarModel> CarModel { get; set; }
        public virtual DbSet<CarSerial> CarSerial { get; set; }
        public virtual DbSet<ImageUrl> ImageUrl { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Rent> Rent { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserSecret> UserSecret { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advert>()
                .Property(e => e.PriceOfDay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Advert>()
                .HasMany(e => e.Rent)
                .WithOptional(e => e.Advert)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Car>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Car>()
                .Property(e => e.Year)
                .IsFixedLength();

            modelBuilder.Entity<Car>()
                .Property(e => e.KM)
                .IsFixedLength();

            modelBuilder.Entity<Car>()
                .HasMany(e => e.Advert)
                .WithOptional(e => e.Car)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CarMark>()
                .Property(e => e.Name)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CarMark>()
                .HasMany(e => e.CarModel)
                .WithOptional(e => e.CarMark)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CarModel>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<CarModel>()
                .HasMany(e => e.CarSerial)
                .WithOptional(e => e.CarModel)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CarSerial>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<CarSerial>()
                .HasMany(e => e.Car)
                .WithOptional(e => e.CarSerial)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ImageUrl>()
                .Property(e => e.Url)
                .IsFixedLength();

            modelBuilder.Entity<ImageUrl>()
                .HasMany(e => e.Car)
                .WithOptional(e => e.ImageUrl)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Address)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Payment)
                .WithOptional(e => e.User)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserSecret>()
                .Property(e => e.UserName)
                .IsFixedLength();

            modelBuilder.Entity<UserSecret>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<UserSecret>()
                .Property(e => e.SaltString)
                .IsFixedLength();

            modelBuilder.Entity<UserSecret>()
                .HasMany(e => e.User)
                .WithOptional(e => e.UserSecret)
                .WillCascadeOnDelete();
        }
    }
}
