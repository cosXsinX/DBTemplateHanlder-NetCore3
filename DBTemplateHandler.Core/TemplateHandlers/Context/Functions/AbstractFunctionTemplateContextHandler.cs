using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public abstract class AbstractFunctionTemplateContextHandler : AbstractTemplateContextHandler
    {

        DatabaseDescriptor _databaseDescriptionPojo;

        public DatabaseDescriptor getAssociatedDatabaseDescriptionPOJO()
        {
            return _databaseDescriptionPojo;
        }
        public void setAssociatedDatabaseDescriptionPOJO(DatabaseDescriptor pojo)
        {
            _databaseDescriptionPojo = pojo;
        }

        TableDescriptor _tableDescriptionPojo;
        public TableDescriptor getAssociatedTableDescriptorPOJO()
        {
            return _tableDescriptionPojo;
        }
        public void setAssociatedTableDescriptorPOJO(TableDescriptor pojo)
        {
            _tableDescriptionPojo = pojo;
        }

        ColumnDescriptor _columnDescriptionPojo;
        public ColumnDescriptor getAssocatedColumnDescriptionPOJO()
        {
            return _columnDescriptionPojo;
        }
        public void setAssociatedColumnDescriptionPOJO(ColumnDescriptor pojo)
        {
            _columnDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
            ColumnDescriptor columnDescriptionPojo = getAssocatedColumnDescriptionPOJO();
            TableDescriptor tableDescriptionPojo = getAssociatedTableDescriptorPOJO();
            DatabaseDescriptor databaseDescriptionPojo = getAssociatedDatabaseDescriptionPOJO();
		    return TemplateHandlerNew.
				    HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
						    tableDescriptionPojo, columnDescriptionPojo);
        }
    }
}
