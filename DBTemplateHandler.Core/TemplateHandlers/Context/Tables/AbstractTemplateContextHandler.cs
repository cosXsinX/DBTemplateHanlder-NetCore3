using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractTableTemplateContextHandler : AbstractTemplateContextHandler
    {
        TableDescriptionPOJO _tableDescriptionPojo;
        public TableDescriptionPOJO getAssociatedTableDescriptorPOJO()
        {
            return _tableDescriptionPojo;
        }

        public void setAssociatedTableDescriptorPOJO(TableDescriptionPOJO pojo)
        {
            _tableDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
		    TableDescriptionPOJO descriptionPojo = getAssociatedTableDescriptorPOJO();
		    if(descriptionPojo == null) return StringTrimedContext;
		    DatabaseDescriptionPOJO databaseDescriptionPojo = descriptionPojo.ParentDatabase;
		    return TemplateHandlerNew.
                    HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
                            descriptionPojo, null );
	    }
    }
}
