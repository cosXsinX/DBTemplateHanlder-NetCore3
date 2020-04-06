using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Service.Contracts.Database;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.ModelImplementation
{
    public class TableModelForTest : ITableModel
    {
        public IList<IColumnModel> Columns { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public IDatabaseModel ParentDatabase { get; set; }
        public IList<IForeignKeyConstraintModel> ForeignKeyConstraints { get; set; }
    }
}
