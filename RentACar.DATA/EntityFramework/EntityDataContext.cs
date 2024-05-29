using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RentACar.DATA.EntityFramework
{
    public partial class EntityDataContext : DbContext
    {
        public EntityDataContext()
            : base("name=EntityDataContext")
        {
        }

        public virtual DbSet<advert> advert { get; set; }
        public virtual DbSet<car> car { get; set; }
        public virtual DbSet<carmark> carmark { get; set; }
        public virtual DbSet<carmodel> carmodel { get; set; }
        public virtual DbSet<carserial> carserial { get; set; }
        public virtual DbSet<rent> rent { get; set; }
        public virtual DbSet<rentrequest> rentrequest { get; set; }
        public virtual DbSet<user> user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<car>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<car>()
                .Property(e => e.Year)
                .IsUnicode(false);

            modelBuilder.Entity<car>()
                .Property(e => e.KM)
                .IsUnicode(false);

            modelBuilder.Entity<carmodel>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<carserial>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<carserial>()
                .Property(e => e.CarModelID)
                .IsUnicode(false);

            modelBuilder.Entity<rent>()
                .Property(e => e.RentRequestID)
                .IsUnicode(false);

            modelBuilder.Entity<rent>()
                .Property(e => e.RentDateState)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.TC)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Adress)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.VerifyConfirmCode)
                .IsUnicode(false);
        }
    }
}
