using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplateHandlerBuilder
    {
        public static ITemplateHandler Build(IList<ITypeMapping> typeMappings)
        {
            return new TemplateHandler(typeMappings);
        }
    }
}
