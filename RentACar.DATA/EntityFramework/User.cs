namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Advert = new HashSet<Advert>();
            Payment = new HashSet<Payment>();
        }

        public int ID { get; set; }

        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(25)]
        public string LastName { get; set; }

        public int? UserSecretID { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        public int? VerifyState { get; set; }

        public int? Age { get; set; }

        [StringLength(13)]
        public string Phone { get; set; }

        public int? Gender { get; set; }

        public int? TC { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public int ImageUrlID { get; set; }

        public int? RentRequestWarrant { get; set; }

        public int? VerifyConfirmCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Advert> Advert { get; set; }

        public virtual ImageUrl ImageUrl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }

        public virtual UserSecret UserSecret { get; set; }
    }
}
