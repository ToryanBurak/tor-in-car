using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.ExtendedDataContext.EFContext
{
    public class AuditInfo
    {
        #region Fields

        public static string[] Fields
        {
            get
            {
                string[] fieldArr = new string[6]{
                    CreateUserID,
                    CreateTime,
                    UpdateUserID,
                    UpdateTime,
                    ActionUserID,
                    ActionTime
                };

                return fieldArr;
            }
        }

        public static string CreateUserID = "CreateUserID";
        public static string CreateTime = "CreateTime";
        public static string UpdateUserID = "UpdateUserID";
        public static string UpdateTime = "UpdateTime";
        public static string ActionTime = "ActionTime";
        public static string ActionUserID = "ActionUserID";
        public static string Action = "Action";
        public DbContext dataContext;
        public int userId;

        #endregion Fields

        public AuditInfo(DbContext dataContext, int userId)
        {
            this.dataContext = dataContext;
            this.userId = userId;
        }

        //private void setIntProperty(object tableName, string propertyName, int propertyValue)
        //{
        //    PropertyInfo propertyInfo = tableName.GetType().GetProperty(propertyName);

        //    if (propertyInfo != null)
        //    {
        //        tableName.GetType().GetProperty(propertyName).SetValue(tableName, propertyValue, null);
        //    }
        //}

        //private void setDateTimeProperty(object tableName, string propertyName, DateTime propertyValue, bool nullCheck = false)
        //{
        //    PropertyInfo propertyInfo = tableName.GetType().GetProperty(propertyName);

        //    if (propertyInfo != null)
        //    {
        //        if (nullCheck == true)
        //        {
        //            DateTime? date = propertyInfo.GetValue(tableName) as DateTime?;
        //            if (date.HasValue == false || date.Value.Date == DateTime.MinValue.Date)
        //            {
        //                propertyInfo.SetValue(tableName, propertyValue, null);
        //            }
        //        }
        //        else
        //        {
        //            propertyInfo.SetValue(tableName, propertyValue, null);
        //        }
        //    }
        //}

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

        public void setUpdateColumns()
        {
            var changes = dataContext.ChangeTracker.Entries();

            foreach (object item in changes.Where(p => p.State == EntityState.Modified))
            {
                setIntProperty(item, UpdateUserID, userId);
                setDateTimeProperty(item, UpdateTime, DateTime.UtcNow);
            }
            foreach (object item in changes.Where(p => p.State == EntityState.Added))
            {
                setIntProperty(item, UpdateUserID, userId);
                setDateTimeProperty(item, UpdateTime, DateTime.UtcNow, true);
                setIntProperty(item, CreateUserID, userId);
                setDateTimeProperty(item, CreateTime, DateTime.UtcNow, true);
            }
        }
    }
}
