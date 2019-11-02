using System.Collections.Generic;
using DBTemplateHandler.Core.Database;

namespace DBTemplateHandler.Core.Template
{
    public interface IDatabaseTemplateHandlerInputModel
    {
        IDatabaseModel DatabaseModel { get; set; }
        IList<ITemplateModel> TemplateModels { get; set; }
    }
}