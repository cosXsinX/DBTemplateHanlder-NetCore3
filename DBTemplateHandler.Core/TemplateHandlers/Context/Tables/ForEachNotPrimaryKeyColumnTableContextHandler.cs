using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachNotPrimaryKeyColumnTableContextHandler : AbstractTableTemplateContextHandler
    {


        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH[";
        public const String END_CONTEXT_WORD = "]::}";



        public override String getStartContextStringWrapper()
        {
            return START_CONTEXT_WORD;
        }

        public override String getEndContextStringWrapper()
        {
            return END_CONTEXT_WORD;
        }

        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception("The provided StringContext is null");
            TableDescriptionPOJO descriptionPojo = getAssociatedTableDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedTableDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TableColumnDescriptionPOJO currentColumn in descriptionPojo.get_ColumnsList())
            {
                if (!currentColumn.is_PrimaryKey())
                {
                    String treated =
                            TemplateHandlerNew.HandleTableColumnTemplate
                                (TrimedStringContext, currentColumn);
                    treated = TemplateHandlerNew.
                            HandleFunctionTemplate
                                            (treated, descriptionPojo.ParentDatabase,
                                                    descriptionPojo, currentColumn);
                    stringBuilder.Append(treated);
                }
            }
            return stringBuilder.ToString();
        }


        public override boolean isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }
}
