using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Domain
{
    public class BaseDO
    {
        public int ID { get; set; }

        public int CreateUserID { get; set; }

        public DateTime CreateTime { get; set; }

        public int? UpdateUserID { get; set; }

        public DateTime? UpdateTime { get; set; }

    }
}
