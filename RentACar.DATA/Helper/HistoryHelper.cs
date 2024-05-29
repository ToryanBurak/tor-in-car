using RentACar.ExtendedDataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.DATA.Helper
{
    public class HistoryHelper
    {
        public static CommitDBResult CommitChanges(DbContext _dataContext, int _userID)
        {
            CommitDBResult commitDBResult = CommitDBResult.Success;
            //CheckForInvalidVersions(_dataContext);
            SetAuditInfo(_dataContext, _userID);
            WriteUpdatesAndDeletes(_dataContext, _userID);

            _dataContext.SaveChanges();
            //SubmitChanges(_dataContext);

            // History de current item union ile gerçek tabloya bağlanılarak çekiliyor. Dolayısıyla insert changeset ini history ye atmaya gerek kalmıyor.
            //ProcessInsertItems(_dataContext, _historyDataContext, _userID);

            return commitDBResult;
        }
        private static void SetAuditInfo(DbContext _dataContext, int _userID)
        {
            ExtendedDataContext.EFContext.AuditInfo auditInfo = new ExtendedDataContext.EFContext.AuditInfo(_dataContext, _userID);
            auditInfo.setUpdateColumns();
        }

        public static void WriteUpdatesAndDeletes(DbContext _dataContext, int _userID)
        {
            //EntitiesToInsert = _dataContext.GetChangeSet().Inserts;
            //SetAuditInfo(_dataContext, _userID);
            ProcessUpdateItems(_dataContext, _userID);
            ProcessDeleteItems(_dataContext, _userID);
        }
        private static void ProcessUpdateItems(DbContext _dataContext, int _userID)
        {
            _dataContext.ChangeTracker.DetectChanges();
            var entries = _dataContext.ChangeTracker.Entries();
            foreach (var item in entries.Where(p => p.State == EntityState.Modified))
            {
                var originalValues = item.OriginalValues;
                object originalItem = originalValues.ToObject();

                CommitActionType actionType = CommitActionType.Update;
                var entityName = item.Entity.GetType().Name;
                CopyTableColumns(originalItem, actionType, _dataContext, entityName, _userID);
            }
        }
        private static void ProcessDeleteItems(DbContext _dataContext, int _userID)
        {
            //ChangeSet CurrentChangeSet = _dataContext.GetChangeSet();
            //foreach (var item in CurrentChangeSet.Deletes)
            //{
            //    CopyTableColumns(item, CommitActionType.Delete, _historyDataContext, _userID);
            //}
            var entries = _dataContext.ChangeTracker.Entries();
            foreach (var entry in entries.Where(p => p.State == EntityState.Deleted))
            {
                var originalValues = entry.OriginalValues;
                object originalItem = originalValues.ToObject();

                var entityName = entry.Entity.GetType().Name;

                CopyTableColumns(originalItem, CommitActionType.Delete, _dataContext, entityName, _userID);
            }
        }
        private static void CopyTableColumns(object item, CommitActionType actionType, DbContext _targetDataContext, string name, int _userID)
        {
            try
            {
                string typeName = item.GetType().AssemblyQualifiedName.Replace(item.GetType().FullName, item.GetType().FullName + "_History");
                //typeName = name + "_History";
                var entityBaseType = item.GetType().BaseType;
                typeName = "Entegral.ECommerce.DataContext.EntityFramework." + entityBaseType.Name + "_History";
                Type historyType = Type.GetType(typeName);

                if (historyType != null)
                {
                    ObjectHandle instance = Activator.CreateInstance(historyType.Assembly.FullName, typeName);

                    string tableName = entityBaseType.Name + "_History";
                    List<PropertyInfo> columns = new List<PropertyInfo>();

                    foreach (PropertyInfo column in item.GetType().GetProperties())
                    {
                        //if (AllowedTypes.Contains(column.PropertyType.BaseType.FullName))
                        columns.Add(column);
                    }

                    foreach (var column in instance.Unwrap().GetType().GetProperties())
                    {
                        if (columns.FirstOrDefault(q => q.Name == column.Name) != null)
                        {
                            string colName = columns.FirstOrDefault(q => q.Name == column.Name).Name;
                            object val = item.GetType().GetProperty(colName).GetValue(item, null);
                            //look val objesi reflection'la guid olarak dönüştürülüyor. Nasıl bir çözüm uygulanacak?
                            if (val != null && item.GetType().GetProperty(colName).PropertyType.FullName.Contains("System.Guid") && column.PropertyType.FullName == "System.String")
                            {
                                column.SetValue(instance.Unwrap(), val.ToString(), null);

                            }
                            else
                            {
                                column.SetValue(instance.Unwrap(), val, null);
                            }
                        }
                    }

                    setProperty(instance.Unwrap(), ExtendedDataContext.EFContext.AuditInfo.ActionTime, DateTime.UtcNow);
                    setProperty(instance.Unwrap(), ExtendedDataContext.EFContext.AuditInfo.ActionUserID, _userID);
                    setProperty(instance.Unwrap(), ExtendedDataContext.EFContext.AuditInfo.Action, Convert.ToInt32(actionType));

                    _targetDataContext.Set(instance.Unwrap().GetType()).Add(instance.Unwrap());
                }
            }
            catch (Exception ex)
            {

            }

        }
        private static void setProperty(object tableName, string propertyName, DateTime propertyValue)
        {
            PropertyInfo prop = tableName.GetType().GetProperty(propertyName);

            if (prop != null)
            {
                tableName.GetType().GetProperty(propertyName).SetValue(tableName, propertyValue, null);
            }
        }
        private static void setProperty(object tableName, string propertyName, int propertyValue)
        {
            PropertyInfo insertBy = tableName.GetType().GetProperty(propertyName);

            if (insertBy != null)
            {
                tableName.GetType().GetProperty(propertyName).SetValue(tableName, propertyValue, null);
            }
        }

    }
}
