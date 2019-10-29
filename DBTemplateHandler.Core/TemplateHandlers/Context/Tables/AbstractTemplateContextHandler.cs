using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractTableTemplateContextHandler : AbstractTemplateContextHandler, ITableTemplateContextHandler
    {
        TableDescriptor _tableDescriptionPojo;
        public TableDescriptor getAssociatedTableDescriptorPOJO()
        {
            return _tableDescriptionPojo;
        }

        public void setAssociatedTableDescriptorPOJO(TableDescriptor pojo)
        {
            _tableDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
		    TableDescriptor descriptionPojo = getAssociatedTableDescriptorPOJO();
		    if(descriptionPojo == null) return StringTrimedContext;
		    DatabaseDescriptor databaseDescriptionPojo = descriptionPojo.ParentDatabase;
		    return TemplateHandlerNew.
                    HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
                            descriptionPojo, null );
	    }
    }
}
