using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Functions
{
    public abstract class AbstractFunctionTemplateContextHandler : AbstractTemplateContextHandler
    {

        DatabaseDescriptionPOJO _databaseDescriptionPojo;

        public DatabaseDescriptionPOJO getAssociatedDatabaseDescriptionPOJO()
        {
            return _databaseDescriptionPojo;
        }
        public void setAssociatedDatabaseDescriptionPOJO(DatabaseDescriptionPOJO pojo)
        {
            _databaseDescriptionPojo = pojo;
        }

        TableDescriptionPOJO _tableDescriptionPojo;
        public TableDescriptionPOJO getAssociatedTableDescriptorPOJO()
        {
            return _tableDescriptionPojo;
        }
        public void setAssociatedTableDescriptorPOJO(TableDescriptionPOJO pojo)
        {
            _tableDescriptionPojo = pojo;
        }

        TableColumnDescriptionPOJO _columnDescriptionPojo;
        public TableColumnDescriptionPOJO getAssocatedColumnDescriptionPOJO()
        {
            return _columnDescriptionPojo;
        }
        public void setAssociatedColumnDescriptionPOJO(TableColumnDescriptionPOJO pojo)
        {
            _columnDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
            TableColumnDescriptionPOJO columnDescriptionPojo = getAssocatedColumnDescriptionPOJO();
            TableDescriptionPOJO tableDescriptionPojo = getAssociatedTableDescriptorPOJO();
            DatabaseDescriptionPOJO databaseDescriptionPojo = getAssociatedDatabaseDescriptionPOJO();
		    return TemplateHandlerNew.
				    HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
						    tableDescriptionPojo, columnDescriptionPojo);
        }
    }
}
