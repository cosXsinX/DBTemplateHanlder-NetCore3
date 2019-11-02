using System.Collections.Generic;

namespace DBTemplateHandler.Core.Database
{
    public interface ITableModel
    {
        List<IColumnModel> Columns { get; set; }
        string Name { get; set; }
        IDatabaseModel ParentDatabase { get; set; }
    }
}