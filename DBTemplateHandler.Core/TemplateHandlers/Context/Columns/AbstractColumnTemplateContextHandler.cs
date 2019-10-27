using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Columns
{
    public abstract class AbstractColumnTemplateContextHandler : AbstractTemplateContextHandler
    {
        public AbstractColumnTemplateContextHandler()
        {

        }

        ColumnDescriptor _tableColumnDescriptionPojo;
        public ColumnDescriptor getAssociatedColumnDescriptorPOJO()
        {
            return _tableColumnDescriptionPojo;
        }

        public void setAssociatedColumnDescriptorPOJO(ColumnDescriptor pojo)
        {
            _tableColumnDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
            if (StringTrimedContext == null) return null;
            ColumnDescriptor descriptionPojo = getAssociatedColumnDescriptorPOJO();
            if (descriptionPojo == null) return StringTrimedContext;
            TableDescriptor tableDescriptionPojo = descriptionPojo.ParentTable;
            DatabaseDescriptor databaseDescriptionPojo = null;
            if (tableDescriptionPojo != null)
                databaseDescriptionPojo = tableDescriptionPojo.ParentDatabase;
            return TemplateHandlerNew.
                HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
                        tableDescriptionPojo, descriptionPojo);
        }
	}
}
