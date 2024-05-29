using AutoMapper;
using RentACar.DATA.EntityFramework;
using RentACar.Domain.Advert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.BL
{
    public class BLInitializer
    {
        public static void Initialize()
        {
            InitializeAutoMapper();
        }
        
        private static void InitializeAutoMapper()
        {
            Mapper.CreateMap<Advert, AdvertDO>().ReverseMap();
        }
    }
}
