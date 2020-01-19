using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public interface IDatabaseModel
    {
        string TypeSetName { get; set; }
        string Name { get; set; }
        string ConnectionString { get; set; }
        IList<ITableModel> Tables { get; set; }
    }
}