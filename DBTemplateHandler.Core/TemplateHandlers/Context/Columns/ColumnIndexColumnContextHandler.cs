using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        private const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX";
        private const String END_CONTEXT_WORD = "::}";

        public const String TEMPLATE_TABLE_WORD = START_CONTEXT_WORD + END_CONTEXT_WORD;


        public override String StartContext
        {
            get => START_CONTEXT_WORD;
        }

        public override String EndContext
        {
            get => END_CONTEXT_WORD;
        }

        private readonly static string ZeroAsString = Convert.ToString(0);
        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception("The provided StringContext is null");
            ColumnDescriptor descriptionPojo = getAssociatedColumnDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedColumnDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception("There is a problem with the provided StringContext :'" + StringContext + "' to the suited word '" + (START_CONTEXT_WORD + END_CONTEXT_WORD) + "'");
            if (descriptionPojo.ParentTable == null)
                return ZeroAsString;
            List<ColumnDescriptor> columnList =
                    descriptionPojo.ParentTable.Columns;
            return Convert.ToString(columnList.IndexOf(descriptionPojo));
        }

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return true;
        }

    }
}
