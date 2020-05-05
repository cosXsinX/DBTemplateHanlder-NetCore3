using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public class ForeignKeyConstraintModel : IForeignKeyConstraintModel
    {
        public IList<IForeignKeyConstraintElementModel> Elements { get; set; }
        public string ConstraintName { get; set; }
        public ITableModel ParentTable { get; set; }
    }
}
