﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Persistance.Serializable;
using System.Linq;

namespace DBTemplateHandler.Persistance.Conversion
{
    public class DatabaseModelConverter
    {
        public PersistableDatabaseModel ToPersistable(IDatabaseModel converted)
        {
            if (converted == null) return null;
            var result = new PersistableDatabaseModel();
            result.Name = converted.Name;
            result.ConnectionString = converted.ConnectionString;
            result.TypeSetName = converted.TypeSetName;
            if (converted.Tables != null)
            {
                result.Tables = converted.Tables.Select(ToPersistable).ToList();
            }
            return result;
        }

        public PersistableTableModel ToPersistable(ITableModel converted)
        {
            var result = new PersistableTableModel();
            result.Name = converted.Name;
            if(converted.Columns != null)
            {
                result.Columns = converted.Columns.Select(ToPersistable).ToList();
            }
            return result;
        }

        public PersistableColumnModel ToPersistable(IColumnModel converted)
        {
            var result = new PersistableColumnModel();
            result.IsAutoGeneratedValue = converted.IsAutoGeneratedValue;
            result.IsNotNull = converted.IsNotNull;
            result.IsPrimaryKey = converted.IsPrimaryKey;
            result.Name = converted.Name;
            result.Type = converted.Type;
            return result;
        }

        public IDatabaseModel ToUnPersisted(PersistableDatabaseModel converted)
        {
            if (converted == null) return null;
            var result = new DatabaseModel();
            result.Name = converted.Name;
            result.ConnectionString = converted.ConnectionString;
            result.TypeSetName = converted.TypeSetName;
            if (converted.Tables != null)
            {
                result.Tables = converted.Tables.Select(ToUnPersisted).ToList();
            }
            return result;
        }

        public ITableModel ToUnPersisted(PersistableTableModel converted)
        {
            var result = new TableModel();
            result.Name = converted.Name;
            if (converted.Columns != null)
            {
                result.Columns = converted.Columns.Select(ToUnPersisted).ToList();
            }
            return result;
        }

        public IColumnModel ToUnPersisted(PersistableColumnModel converted)
        {
            var result = new ColumnModel();
            result.IsAutoGeneratedValue = converted.IsAutoGeneratedValue;
            result.IsNotNull = converted.IsNotNull;
            result.IsPrimaryKey = converted.IsPrimaryKey;
            result.Name = converted.Name;
            result.Type = converted.Type;
            return result;
        }
    }
}
