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
        string HandleDatabaseTemplate(string templateString, IDatabaseModel databaseModel);
        string HandleFunctionTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnDescriptionPojo);
        string HandleTableColumnTemplate(string templateString, IColumnModel descriptionPOJO);
        string HandleTableTemplate(string templateString, ITableModel tableModel);
        string HandleTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnModel);
        void OverwriteTypeMapping(IList<ITypeMapping> typeMappings);
    }
}