namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Car")]
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            Advert = new HashSet<Advert>();
        }

        public int ID { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        public int? Fuel { get; set; }

        public int? Gear { get; set; }

        [StringLength(10)]
        public string KM { get; set; }

        public int? Case { get; set; }

        public int? HP { get; set; }

        public int? CC { get; set; }

        public int? Color { get; set; }

        public int? ImageUrlID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime UpdateTime { get; set; }

        public int UpdateUserID { get; set; }

        public int? CarSerialID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Advert> Advert { get; set; }

        public virtual CarSerial CarSerial { get; set; }

        public virtual ImageUrl ImageUrl { get; set; }
    }
}
