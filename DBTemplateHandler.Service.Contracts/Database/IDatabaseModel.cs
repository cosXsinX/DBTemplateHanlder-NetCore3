using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public interface IDatabaseModel
    {
        string Name { get; set; }
        List<ITableModel> Tables { get; set; }
    }
}