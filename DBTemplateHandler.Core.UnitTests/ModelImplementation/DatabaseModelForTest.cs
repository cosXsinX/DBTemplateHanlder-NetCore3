using DBTemplateHandler.Core.Database;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.ModelImplementation
{
    public class DatabaseModelForTest : IDatabaseModel
    {
        public string TypeSetName { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public IList<ITableModel> Tables { get; set; }
    }
}
