namespace RentACar.DATA.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Payment")]
    public partial class Payment
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        public int? PaymentChannel { get; set; }

        public int? PaymentStatus { get; set; }

        public virtual User User { get; set; }
    }
}
