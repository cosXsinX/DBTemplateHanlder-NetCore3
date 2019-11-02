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
        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '" + (START_CONTEXT_WORD + END_CONTEXT_WORD) + "'");
            if (columnModel.ParentTable == null)
                return ZeroAsString;
            List<ColumnModel> columnList =
                    columnModel.ParentTable.Columns;
            return Convert.ToString(columnList.IndexOf(columnModel));
        }

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return true;
        }

    }
}
