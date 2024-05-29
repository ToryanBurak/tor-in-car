namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rent")]
    public partial class Rent
    {
        public int ID { get; set; }

        public int? AdvertID { get; set; }

        public int? RentDay { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? ConfirmState { get; set; }

        public int PaymentID { get; set; }

        public int? PaymentState { get; set; }

        public virtual Advert Advert { get; set; }
    }
}
