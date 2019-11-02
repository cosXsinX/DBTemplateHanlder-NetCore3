using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class TableModel
    {
        public TableModel(string name)
        {
            Name = name;
        }
        public String Name { get; set; }
        public List<ColumnModel> Columns { get; set; }

        //Template handler specific properties
        public DatabaseModel ParentDatabase;
	
	    public void UpdateContainedColumnsParentReference()
        {
            foreach (ColumnModel currentColumn in Columns)
            {
                currentColumn.ParentTable = this;
            }
        }
    }
}
