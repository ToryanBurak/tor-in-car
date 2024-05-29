namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarMark")]
    public partial class CarMark
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarMark()
        {
            CarModel = new HashSet<CarModel>();
        }

        public int ID { get; set; }

        [StringLength(15)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarModel> CarModel { get; set; }
    }
}
