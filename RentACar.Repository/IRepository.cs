using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository
{
    interface IRepository
    {
        object GetById(int id);
        IQueryable GetAll();
        void InsertOnSubmit(object entity);
        void DeleteOnSubmit(object entity);
        //[Obsolete("Units of Work should be managed externally to the Repository.")]
        //void SubmitChanges();
    }

    interface IRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllActive();
        T GetByCode(string code);
        T GetSingleByCode(string code);
        void InsertOnSubmit(T entity);
        void DeleteOnSubmit(T entity);
        void DeleteByIdListOnSubmit(List<int> IDs);
        void DeleteByIdOnSubmit(int ID);
        void UpdateByIdOnSubmit(T entity);
        void DestroyRepository();
    }
}
