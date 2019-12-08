using System.Collections.Generic;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Service.Contracts.TypeMapping;

namespace DBTemplateHandler.Core.Template
{
    public interface IDatabaseTemplateHandlerInputModel
    {
        IDatabaseModel DatabaseModel { get; set; }
        IList<ITemplateModel> TemplateModels { get; set; }
        IList<ITypeMapping> typeMappings { get; set; }
    }
}