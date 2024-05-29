namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarModel")]
    public partial class CarModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarModel()
        {
            CarSerial = new HashSet<CarSerial>();
        }

        public int ID { get; set; }

        [StringLength(15)]
        public string Name { get; set; }

        public int? CarMarkID { get; set; }

        public virtual CarMark CarMark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarSerial> CarSerial { get; set; }
    }
}
