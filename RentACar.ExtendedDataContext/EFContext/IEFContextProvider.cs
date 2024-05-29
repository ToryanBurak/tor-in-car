using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.ExtendedDataContext.EFContext
{
    public interface IEFContextProvider : IContext<DbContext>
    {

    }
}
