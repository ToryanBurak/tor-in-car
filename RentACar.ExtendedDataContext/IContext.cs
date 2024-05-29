using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.ExtendedDataContext
{
    public interface IContext<Context>
    {
        Context GetDataContext();
        Context GetHistoryDataContext();
        void DestroyContext(bool? disposing = null);
        CommitDBResult CommitChanges(int userId);
        CommitDBResult CommitChangesWithoutHistory(int userId);
    }
}
