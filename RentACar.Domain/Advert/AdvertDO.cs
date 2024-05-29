using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Domain.Advert
{
    public class AdvertDO : BaseDO
    {
        public int CarID { get; set; }

        public decimal PriceOfDay { get; set; }

        public DateTime PublishDate { get; set; }

        [Column(TypeName = "bit")]
        public bool ActivityState { get; set; }
    }
}
