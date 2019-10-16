using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public class DatabaseDescriptionPOJO
    {
        
        private String _databaseNameStr;
        public String getDatabaseNameStr()
        {
            return _databaseNameStr;
        }

        public void setDatabaseNameStr(String databaseNameStr)
        {
            this._databaseNameStr = databaseNameStr;
        }

        private List<TableDescriptionPOJO> tableList;
        public List<TableDescriptionPOJO> getTableList()
        {
            return tableList;
        }

        public void setTableList(List<TableDescriptionPOJO> tableList)
        {
            this.tableList = tableList;
        }

        public void UpdateContainedTablesParentReference()
        {
            foreach (TableDescriptionPOJO currentColumn in tableList)
            {
                currentColumn.ParentDatabase = this;
                currentColumn.UpdateContainedColumnsParentReference();
            }
        }
    }
}
