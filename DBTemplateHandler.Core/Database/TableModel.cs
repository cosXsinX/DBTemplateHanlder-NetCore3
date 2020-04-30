using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public class TableModel : ITableModel
    {

        public TableModel()
        {

        }

        public TableModel(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Schema { get; set; }
        public IList<IColumnModel> Columns { get; set; }

        public IDatabaseModel ParentDatabase { get; set; }
        public IList<IForeignKeyConstraintModel> ForeignKeyConstraints { get; set; }

        public void UpdateContainedColumnsParentReference()
        {
            foreach (ColumnModel currentColumn in Columns)
            {
                currentColumn.ParentTable = this;
            }
        }
    }
}
