using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    [Serializable]
    public class TableModel : ITableModel
    {
        public TableModel(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public IList<IColumnModel> Columns { get; set; }

        public IDatabaseModel ParentDatabase { get; set; }
	
	    public void UpdateContainedColumnsParentReference()
        {
            foreach (ColumnModel currentColumn in Columns)
            {
                currentColumn.ParentTable = this;
            }
        }
    }
}
