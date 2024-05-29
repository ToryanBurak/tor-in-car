using RentACar.ExtendedDataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.EFContextRepository
{
    public class Repository<T> : IRepository<T>, IRepository where T : class
    {
        public readonly DbContext dataContext;
        public readonly IContext<DbContext> dataContextProvider;

        private int UserID { get; set; }
        public Repository(IContext<DbContext> _dataContextProvider)
        {
            dataContextProvider = _dataContextProvider;
            dataContext = dataContextProvider.GetDataContext();

            //look
            //Parametrik olarak lazy loading alınacak.
            dataContext.Configuration.LazyLoadingEnabled = false;
        }

        public Repository(IContext<DbContext> _dataContextProvider, int _userId)
        {
            UserID = _userId;
            dataContextProvider = _dataContextProvider;
            dataContext = dataContextProvider.GetDataContext();

            //look
            //Parametrik olarak lazy loading alınacak.
            dataContext.Configuration.LazyLoadingEnabled = false;
        }

        public IContext<DbContext> DCP
        {
            get
            {
                return dataContextProvider;
            }
        }

        #region Get Methods
        public IQueryable<T> GetAll()
        {
            return dataContext.Set<T>();
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = GetTable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public T GetById(int id)
        {
            return GetByIDAsQueryable(id).SingleOrDefault();
        }

        public IQueryable<T> GetAllActive()
        {
            var itemParameter = Expression.Parameter(typeof(T), "item");
            IQueryable<T> retVal = GetAll();
            retVal = AddIsActiveFilterIfExists(retVal);
            return retVal;
        }

        public T GetByCode(string code)
        {
            return GetByCodeAsQueryable(code).SingleOrDefault();
        }
        public T GetByCodeName(string codeName)
        {
            return GetByCodeNameAsQueryable(codeName).SingleOrDefault();
        }

        public T GetSingleByCode(string code)
        {
            return GetByCodeAsQueryable(code).Single();
        }
        #endregion

        #region Update Methods
        public void UpdateByIdOnSubmit(T entity)
        {
            GetTable().Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;

            //int ID = (int)(entity.GetType().GetProperty("ID").GetValue(entity, null));
            //T existingEntity = GetSingleById(ID);
            //ObjectMapper.CopyObject(entity, existingEntity, AuditInfo.Fields);
        }
        #endregion

        #region Insert Methods
        public void InsertOnSubmit(T entity)
        {
            GetTable().Add(entity);
        }

        public void InsertOnSubmit(object entity)
        {
            InsertOnSubmit((T)entity);
        }

        #endregion

        #region Delete Methods
        public void DeleteByIdListOnSubmit(List<int> IDs)
        {
            foreach (var i in IDs)
            {
                DeleteByIdOnSubmit(i);
            }
        }

        public void DeleteByIdOnSubmit(int ID)
        {
            T existingEntity = GetSingleById(ID);
            DeleteOnSubmit(existingEntity);
        }

        public void DeleteOnSubmit(T entity)
        {
            DbEntityEntry<T> entityEntry = dataContext.Entry(entity);
            if (entityEntry.State == EntityState.Detached)
            {
                GetTable().Attach(entity);
            }
            entityEntry.State = EntityState.Deleted;
            //GetTable().Remove(entity);
        }

        public void DeleteOnSubmit(object entity)
        {
            DeleteOnSubmit((T)entity);
        }

        #endregion

        public void DestroyRepository()
        {
            throw new NotImplementedException();
        }

        #region Private Methods
        private T GetSingleById(int id)
        {
            return GetByIDAsQueryable(id).Single();
        }

        private static bool HasIsActive(Type t)
        {
            var property = t.GetProperty("IsActive");
            bool retVal = (property != null);

            return retVal;
        }

        private IQueryable<T> GetByCodeAsQueryable(string code)
        {
            var itemParameter = Expression.Parameter(typeof(T), "item");
            var whereExpression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(Expression.Property(itemParameter, "Code"), Expression.Constant(code)),
                new[] { itemParameter }
                );
            return GetAll().Where(whereExpression);
        }
        private IQueryable<T> GetByCodeNameAsQueryable(string code)
        {
            var itemParameter = Expression.Parameter(typeof(T), "item");
            var whereExpression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(Expression.Property(itemParameter, "CodeName"), Expression.Constant(code)),
                new[] { itemParameter }
                );
            return GetAll().Where(whereExpression);
        }

        private IQueryable<T> AddIsActiveFilterIfExists(IQueryable<T> query)
        {
            var itemParameter = Expression.Parameter(typeof(T), "item");
            if (HasIsActive(typeof(T)))
            {
                var whereExpression2 = Expression.Lambda<Func<T, bool>>
                    (
                    Expression.Equal(Expression.Property(itemParameter, "IsActive"), Expression.Constant(true)),
                    new[] { itemParameter }
                    );
                query = query.Where(whereExpression2);
            }
            return query;
        }

        private IQueryable<T> GetByIDAsQueryable(int id)
        {
            var itemParameter = Expression.Parameter(typeof(T), "item");
            var whereExpression = Expression.Lambda<Func<T, bool>>
                (
                Expression.Equal(Expression.Property(itemParameter, "ID"), Expression.Constant(id)),
                new[] { itemParameter }
                );
            return GetAll().Where(whereExpression);
        }

        IQueryable IRepository.GetAll()
        {
            return GetAll();
        }

        object IRepository.GetById(int id)
        {
            return GetById(id);
        }

        private DbSet<T> GetTable()
        {
            return dataContext.Set<T>();
        }
        #endregion

    }
}

