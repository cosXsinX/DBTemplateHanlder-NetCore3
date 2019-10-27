using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class TableDescriptor
    {
        public TableDescriptor(string name)
        {
            Name = name;
        }
        public String Name { get; set; }
        public List<ColumnDescriptor> Columns { get; set; }

        //Template handler specific properties
        public DatabaseDescriptor ParentDatabase;
	
	    public void UpdateContainedColumnsParentReference()
        {
            foreach (ColumnDescriptor currentColumn in Columns)
            {
                currentColumn.ParentTable = this;
            }
        }
    }
}
