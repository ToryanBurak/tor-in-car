using AutoMapper;
using RentACar.DATA.EntityFramework;
using RentACar.DATA.EntityFramework.Provider;
using RentACar.Domain.Advert;
using RentACar.Repository.EFContextRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.BL
{
    public class AdvertBL
    {
        public static List<AdvertDO> GetAll()
        {
            using (EComDataContextProvider dcp = new EComDataContextProvider())
            {
                Repository<Advert> repository = new Repository<Advert>(dcp);
                List<Advert> advertList = repository.GetAll().ToList();
                return Mapper.Map<List<Advert>, List<AdvertDO>>(advertList);
            }
            
            
        }
    }
}
