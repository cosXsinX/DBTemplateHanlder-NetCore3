using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class PrimaryColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        private const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:INDEX";
        private const String END_CONTEXT_WORD = "::}";

        public static readonly String TEMPLATE_TABLE_WORD = START_CONTEXT_WORD + END_CONTEXT_WORD;
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        private const int ZeroIndex = 0;
        private readonly static string ZeroIndexAsString = Convert.ToString(ZeroIndex);
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
                return ZeroIndexAsString;
            int currentIndex = ZeroIndex;
            int currentAutoIndex = ZeroIndex;
            List<ColumnDescriptor> columnList =
            descriptionPojo.ParentTable.Columns;
            for (currentIndex = 0; currentIndex < columnList.Count; currentIndex++)
            {
                ColumnDescriptor currentColumn = columnList[currentIndex];
                if (!currentColumn.IsPrimaryKey)
                {
                    if (currentColumn.Equals(descriptionPojo))
                    {
                        return Convert.ToString(currentAutoIndex);
                    }
                    currentAutoIndex++;
                }
            }
            return ZeroIndexAsString;
        }


        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return true;
        }

    }
}
