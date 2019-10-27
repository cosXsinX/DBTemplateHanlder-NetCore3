using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class DatabaseDescriptor
    {
        public String Name { get; set; }
        public List<TableDescriptor> Tables { get; set; }

        public void UpdateContainedTablesParentReference()
        {
            if (Tables == null) return;
            foreach (TableDescriptor currentColumn in Tables)
            {
                currentColumn.ParentDatabase = this;
                currentColumn.UpdateContainedColumnsParentReference();
            }
        }
    }
}
