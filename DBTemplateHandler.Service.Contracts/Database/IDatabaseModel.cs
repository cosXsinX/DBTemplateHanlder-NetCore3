using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public interface IDatabaseModel
    {
        string TypeSetName { get; set; }
        string Name { get; set; }
        IList<ITableModel> Tables { get; set; }
    }
}