using System.Collections.Generic;
using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.Template
{
    public interface IColumnTemplateHandlerInputModel
    {
        ITableModel databaseDescriptor { get; set; }
        IList<ITemplateModel> templateModels { get; set; }
    }
}