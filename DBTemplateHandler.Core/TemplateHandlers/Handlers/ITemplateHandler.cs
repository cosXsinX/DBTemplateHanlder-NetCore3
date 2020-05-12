using System.Collections.Generic;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Service.Contracts.TypeMapping;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public interface ITemplateHandler
    {
        IList<ITypeMapping> TypeMappings { get; }
        IList<ITemplateContextHandlerIdentity> GetAllItemplateContextHandlerIdentity();
        string HandleDatabaseTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleFunctionTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTableColumnTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTableTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTemplate(string templateString, IDatabaseContext databaseContext);
        void OverwriteTypeMapping(IList<ITypeMapping> typeMappings);
    }
}