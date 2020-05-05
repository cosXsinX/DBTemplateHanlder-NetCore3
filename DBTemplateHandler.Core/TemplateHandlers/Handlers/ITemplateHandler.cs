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
        string HandleDatabaseTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleFunctionTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnDescriptionPojo,IForeignKeyConstraintModel foreignKeyConstraint);
        string HandleFunctionTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTableColumnTemplate(string templateString, IColumnModel descriptionPOJO);
        string HandleTableColumnTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTableTemplate(string templateString, ITableModel tableModel);
        string HandleTableTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTemplate(string templateString, IDatabaseContext databaseContext);
        string HandleTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnModel, IForeignKeyConstraintModel foreignKeyConstraint);
        void OverwriteTypeMapping(IList<ITypeMapping> typeMappings);
    }
}