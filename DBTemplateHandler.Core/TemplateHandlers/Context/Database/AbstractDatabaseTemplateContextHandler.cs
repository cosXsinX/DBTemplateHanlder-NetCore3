using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public abstract class AbstractDatabaseTemplateContextHandler : AbstractTemplateContextHandler, IDatabaseTemplateContextHandler
    {

	    DatabaseDescriptor _databaseDescriptionPojo;
        public DatabaseDescriptor getAssociatedDatabaseDescriptorPOJO()
        {
            return _databaseDescriptionPojo;
        }

        public void setAssociatedDatabaseDescriptorPOJO(DatabaseDescriptor pojo)
        {
            _databaseDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
            DatabaseDescriptor descriptionPojo = getAssociatedDatabaseDescriptorPOJO();
		    if(descriptionPojo == null) return StringTrimedContext;
		    return TemplateHandlerNew.
				    HandleTemplate(StringTrimedContext, descriptionPojo,
						    null, null);
        }
    }
}
