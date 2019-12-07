using System.Collections.Generic;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Service.Contracts.TypeMapping;

namespace DBTemplateHandler.Core.Template.Contract
{
    public interface IDatabaseTemplateHandlerInputModel
    {
        IDatabaseModel DatabaseModel { get; set; }
        IList<ITemplateModel> TemplateModels { get; set; }
        IList<ITypeMapping> TypeMappings { get; set; }
    }
}