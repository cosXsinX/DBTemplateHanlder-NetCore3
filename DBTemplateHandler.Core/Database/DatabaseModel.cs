using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class DatabaseModel :IDatabaseModel
    {
        public string Name { get; set; }
        public IList<ITableModel> Tables { get; set; }

        public void UpdateContainedTablesParentReference()
        {
            if (Tables == null) return;
            foreach (TableModel currentColumn in Tables)
            {
                currentColumn.ParentDatabase = this;
                currentColumn.UpdateContainedColumnsParentReference();
            }
        }
    }
}
