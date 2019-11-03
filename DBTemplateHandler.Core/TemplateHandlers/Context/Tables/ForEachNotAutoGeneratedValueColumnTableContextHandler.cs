﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachNotAutoGeneratedValueColumnTableContextHandler : AbstractTableTemplateContextHandler
    {


        public const string START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH[";
        public const string END_CONTEXT_WORD = "]::}";
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        public override string processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel table = TableModel;
            if (table == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(ColumnModel currentColumn in table.Columns)
            {
                if (!currentColumn.IsAutoGeneratedValue)
                {
                    string treated =
                            TemplateHandlerNew.HandleTableColumnTemplate
                                (TrimedStringContext, currentColumn);
                    treated = TemplateHandlerNew.
                            HandleFunctionTemplate
                                            (treated, table.ParentDatabase,
                                                    table, currentColumn);
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
