using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public interface IDatabaseModel
    {
        string Name { get; set; }
        IList<ITableModel> Tables { get; set; }
    }
}