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

        TableColumnDescriptionPOJO _tableColumnDescriptionPojo;
        public TableColumnDescriptionPOJO getAssociatedColumnDescriptorPOJO()
        {
            return _tableColumnDescriptionPojo;
        }

        public void setAssociatedColumnDescriptorPOJO(TableColumnDescriptionPOJO pojo)
        {
            _tableColumnDescriptionPojo = pojo;
        }

        public override String HandleTrimedContext(String StringTrimedContext)
        {
            if (StringTrimedContext == null) return null;
            TableColumnDescriptionPOJO descriptionPojo = getAssociatedColumnDescriptorPOJO();
            if (descriptionPojo == null) return StringTrimedContext;
            TableDescriptionPOJO tableDescriptionPojo = descriptionPojo.ParentTable;
            DatabaseDescriptionPOJO databaseDescriptionPojo = null;
            if (tableDescriptionPojo != null)
                databaseDescriptionPojo = tableDescriptionPojo.ParentDatabase;
            return TemplateHandlerNew.
                HandleTemplate(StringTrimedContext, databaseDescriptionPojo,
                        tableDescriptionPojo, descriptionPojo);
        }
	}
}
