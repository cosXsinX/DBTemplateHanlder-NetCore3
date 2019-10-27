using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnNotNullValueColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NOT:NULL(";
        public const String END_CONTEXT_WORD = "):::}";



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
            ColumnDescriptor descriptionPojo = getAssociatedColumnDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedColumnDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (descriptionPojo.IsNotNull)
            {
                return HandleTrimedContext(TrimedStringContext);
            }
            else return "";
        }


        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }

    }
}
