using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class DatabaseModel
    {
        public string Name { get; set; }
        public List<TableModel> Tables { get; set; }

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
