using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnNameColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        public const string START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME";
        public const string END_CONTEXT_WORD = "::}";

        public const string TEMPLATE_TABLE_WORD = START_CONTEXT_WORD + END_CONTEXT_WORD;


        public override string StartContext
        {
            get => START_CONTEXT_WORD;
        }

        public override string EndContext
        {
            get => END_CONTEXT_WORD;
        }

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{TEMPLATE_TABLE_WORD}'");
            return columnModel.Name;
        }

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return true;
        }

    }
}
