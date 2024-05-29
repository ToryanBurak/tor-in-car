namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarSerial")]
    public partial class CarSerial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarSerial()
        {
            Car = new HashSet<Car>();
        }

        public int ID { get; set; }

        [StringLength(15)]
        public string Name { get; set; }

        public int? CarModelID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Car { get; set; }

        public virtual CarModel CarModel { get; set; }
    }
}
