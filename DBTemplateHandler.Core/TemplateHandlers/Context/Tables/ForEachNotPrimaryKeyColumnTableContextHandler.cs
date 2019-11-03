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
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel descriptionPojo = TableModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ColumnModel currentColumn in descriptionPojo.Columns)
            {
                if (!currentColumn.IsPrimaryKey)
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


        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }
}
