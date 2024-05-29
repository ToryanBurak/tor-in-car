namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Advert")]
    public partial class Advert
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Advert()
        {
            Rent = new HashSet<Rent>();
        }

        public int ID { get; set; }

        public int? CarID { get; set; }

        public decimal? PriceOfDay { get; set; }

        public DateTime? PublishDate { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime UpdateTime { get; set; }

        public int UpdateUserID { get; set; }

        public int? UserID { get; set; }

        public virtual Car Car { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rent> Rent { get; set; }
    }
}
