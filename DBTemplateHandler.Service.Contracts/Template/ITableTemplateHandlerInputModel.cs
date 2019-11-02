using System.Collections.Generic;
using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.Template
{
    public interface ITableTemplateHandlerInputModel
    {
        ITableModel databaseDescriptor { get; set; }
        IList<ITemplateModel> templateModels { get; set; }
    }
}