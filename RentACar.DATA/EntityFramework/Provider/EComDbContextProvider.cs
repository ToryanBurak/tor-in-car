using RentACar.DATA.Helper;
using RentACar.ExtendedDataContext;
using RentACar.ExtendedDataContext.EFContext;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DATA.EntityFramework.Provider
{
    public class EComDataContextProvider : IEFContextProvider, IDisposable
    {
        private EntityDbContext _dataContext;
        //private EcomHistoryDataContext _historyDataContext;
        //private static readonly string connectionStringName = "EcomDataContext";
        private bool disposed = false;

        public static string CreateUserID = "CreateUserID";
        public static string CreateTime = "CreateTime";
        public static string UpdateUserID = "UpdateUserID";
        public static string UpdateTime = "UpdateTime";

        public static EComDataContextProvider Instance
        {
            get
            {
                return new EComDataContextProvider();
            }
        }

        public EntityDbContext GetEcomDataContext()
        {
            return (EntityDbContext)GetDataContext();
        }

        public EntityDbContext GetEcomFeedDataContext()
        {
            return (EntityDbContext)GetFeedDataContext();
        }

        public DbContext GetDataContext()
        {
            // TODO: Connection string icin encryption yapılacak...
            if (_dataContext == null)
                _dataContext = new EntityDbContext();
            //_dataContext = new EcomDataContext(ConfigurationManager.ConnectionStrings[connectionStringName].ToString());
            ((IObjectContextAdapter)_dataContext).ObjectContext.CommandTimeout = 300;
            return _dataContext;
        }

        public DbContext GetFeedDataContext()
        {
            // TODO: Connection string icin encryption yapılacak...
            if (_dataContext == null)
                _dataContext = new EntityDbContext();

            ((IObjectContextAdapter)_dataContext).ObjectContext.CommandTimeout = 300;
            return _dataContext;
        }

        //public DbContext GetHistoryDataContext()
        //{
        //    // TODO: Connection string icin encryption yapılacak...
        //    if (_historyDataContext == null)
        //        _historyDataContext = new EcomHistoryDataContext(ConfigurationManager.ConnectionStrings[connectionStringName].ToString());

        //    return _historyDataContext;
        //}

        //look HistoryDataContext tabloları mySql de schema olmamasından kaynaklı EcomDataContext ine dönüştürüldü.
        public DbContext GetHistoryDataContext()
        {
            // TODO: Connection string icin encryption yapılacak...
            if (_dataContext == null)
                _dataContext = new EntityDbContext();

            return _dataContext;
        }


        public void DestroyContext(bool? disposing = null)
        {
            if (_dataContext != null)
            {
                if (!disposed)
                {
                    if (disposing != null && disposing == true)
                    {
                        _dataContext.Dispose();
                    }
                }
                disposed = true;
            }
        }

        public CommitDBResult CommitChanges(int UserID)
        {
            //Look
            //return DBContextHelper.CommitChanges(this.GetDataContext(), this.GetHistoryDataContext(), UserID);
            HistoryHelper.CommitChanges(_dataContext, UserID);
            //SaveChanges(this.GetHistoryDataContext());
            //setUpdateColumns(UserID);
            //_dataContext.SaveChanges();
            CommitDBResult commitDBResult = CommitDBResult.Success;

            // Transaction işlemleri burada ele alınabilir veya Identity Map kurumsal tasarım kalıbı kullanılarak
            // sadece değişen alanları güncellemeyide sağlayabiliriz.
            return commitDBResult;
        }
        public void setUpdateColumns(int UserID)
        {
            var changes = _dataContext.ChangeTracker.Entries();
            var updateList = changes.Where(s => s.State == EntityState.Modified);
            var insertList = changes.Where(s => s.State == EntityState.Added);
            var dt = DateTime.UtcNow;
            foreach (object item in updateList)
            {
                setIntProperty(item, UpdateUserID, UserID);
                setDateTimeProperty(item, UpdateTime, dt);
            }
            foreach (object item in insertList)
            {
                setIntProperty(item, UpdateUserID, UserID);
                setDateTimeProperty(item, UpdateTime, dt);
                setIntProperty(item, CreateUserID, UserID);
                setDateTimeProperty(item, CreateTime, dt);
            }
        }
        private void setDateTimeProperty(object tableName, string propertyName, DateTime propertyValue, bool nullCheck = false)
        {
            var haveProperty = ((DbEntityEntry)tableName).CurrentValues.PropertyNames.Any(s => s == propertyName);

            if (haveProperty)
            {
                if (nullCheck == true)
                {
                    ((DbEntityEntry)tableName).CurrentValues[propertyName] = propertyValue;
                }
                else
                {
                    ((DbEntityEntry)tableName).CurrentValues[propertyName] = propertyValue;
                }
            }
        }
        private void setIntProperty(object tableName, string propertyName, int propertyValue)
        {
            var haveProperty = ((DbEntityEntry)tableName).CurrentValues.PropertyNames.Any(s => s == propertyName);
            if (haveProperty)
            {
                ((DbEntityEntry)tableName).CurrentValues[propertyName] = propertyValue;
            }
        }
        public CommitDBResult CommitChangesWithoutHistory(int UserID)
        {
            //return DBContextHelper.CommitChangesWithoutHistory(this.GetDataContext(), UserID);

            try
            {
                setUpdateColumns(UserID);
                _dataContext.SaveChanges();
                CommitDBResult commitDBResult = CommitDBResult.Success;

                // Transaction işlemleri burada ele alınabilir veya Identity Map kurumsal tasarım kalıbı kullanılarak
                // sadece değişen alanları güncellemeyide sağlayabiliriz.
                return commitDBResult;

            }
            catch
            {
                // Burada DbEntityValidationException hatalarını handle edebiliriz.
                CommitDBResult commitDBResult = CommitDBResult.Fail;
                return commitDBResult;
            }
        }

        public void Dispose()
        {
            DestroyContext(true);
            GC.SuppressFinalize(this);
        }

        public void DiscardPendingChanges()
        {
            _dataContext.Dispose();
            _dataContext = new EntityDbContext();
        }

        DbContext IContext<DbContext>.GetHistoryDataContext()
        {
            throw new NotImplementedException();
        }

        #region ExecuteStoredProcedure

        #region WithReturnParameter

        private List<T> ExecuteMsSqlProcedureQuery<T>(string procedureName, KeyValuePair<string, object>[] sqlParameters)
        {
            SqlParameter[] msSqlParameters = new SqlParameter[sqlParameters.Length];
            string parameterNames = "";

            for (int i = 0; i < sqlParameters.Length; i++)
            {
                msSqlParameters[i] = new SqlParameter(sqlParameters[i].Key, sqlParameters[i].Value);
                parameterNames += sqlParameters[i].Key + ',';
            }

            parameterNames = parameterNames.TrimEnd(',');
            string query = "exec " + procedureName + " " + parameterNames;
            return _dataContext.Database.SqlQuery<T>(query, msSqlParameters).ToList();
        }
        #endregion

        #region WithoutReturnParameter


        private void ExecuteMsSqlProcedureQuery(string procedureName, KeyValuePair<string, object>[] sqlParameters)
        {
            SqlParameter[] msSqlParameters = new SqlParameter[sqlParameters.Length];
            string parameterNames = "";

            for (int i = 0; i < sqlParameters.Length; i++)
            {
                msSqlParameters[i] = new SqlParameter(sqlParameters[i].Key, sqlParameters[i].Value);
                parameterNames += sqlParameters[i].Key + ',';
            }

            parameterNames = parameterNames.TrimEnd(',');
            string query = "exec " + procedureName + " " + parameterNames;
            _dataContext.Database.ExecuteSqlCommand(query, msSqlParameters);
        }
        #endregion

        #endregion

        public int SaveChanges(DbContext _dataContext)
        {
            //Set TrackedEntity update columns
            foreach (var entry in _dataContext.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Unchanged && !entry.Entity.GetType().Name.Contains("_History")) //ignore unchanged entities and history tables
                {
                    //entry.Entity = DateTime.UtcNow;
                    //entry.Entity.ModifiedBy = username;
                    //entry.Entity.Version += 1;

                    //add original values to history table (skip if this entity is not yet created)                 
                    if (entry.State != EntityState.Added && entry.Entity.GetType().BaseType != null)
                    {
                        //check the base type exists (actually the derived type e.g. Record)
                        Type entityBaseType = entry.Entity.GetType().BaseType;
                        if (entityBaseType == null)
                            continue;

                        //check there is a history type for this entity type
                        Type entityHistoryType = Type.GetType("Entegral.ECommerce.DataContext.EntityFramework.EcomHistoryModel." + entityBaseType.Name + "_History");
                        if (entityHistoryType == null)
                            continue;

                        //create history object from the original values
                        var history = Activator.CreateInstance(entityHistoryType);
                        foreach (PropertyInfo property in entityHistoryType.GetProperties().Where(p => p.CanWrite && entry.OriginalValues.PropertyNames.Contains(p.Name)))
                            property.SetValue(history, entry.OriginalValues[property.Name], null);

                        //add the history object to the appropriate DbSet
                        MethodInfo method = typeof(DbContext).GetMethod("AddToDbSet");
                        MethodInfo generic = method.MakeGenericMethod(entityHistoryType);
                        generic.Invoke(this, new[] { history });
                    }
                }
            }

            return 1;
        }

        //public void AddToDbSet<T>(T value) where T : class
        //{
        //    PropertyInfo property = GetType().GetProperties().FirstOrDefault(p => p.PropertyType.IsGenericType
        //        && p.PropertyType.Name.StartsWith("DbSet")
        //        && p.PropertyType.GetGenericArguments().Length > 0
        //        && p.PropertyType.GetGenericArguments()[0] == typeof(T));
        //    if (property == null)
        //        return;

        //    ((DbSet<T>)property.GetValue(this, null)).Add(value);
        //}

        public void RejectChanges()
        {
            foreach (var entry in _dataContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
    }
}
