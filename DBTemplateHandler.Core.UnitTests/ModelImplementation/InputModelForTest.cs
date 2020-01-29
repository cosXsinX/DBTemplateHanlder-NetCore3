using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.ModelImplementation
{
    public class InputModelForTest : IDatabaseTemplateHandlerInputModel
    {
        public IDatabaseModel DatabaseModel { get; set; }
        public IList<ITemplateModel> TemplateModels { get; set; }
        public IList<ITypeMapping> typeMappings { get; set; }
    }
}
