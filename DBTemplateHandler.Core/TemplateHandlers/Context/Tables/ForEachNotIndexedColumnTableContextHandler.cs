﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachNotIndexedColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {

        public ForEachNotIndexedColumnTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:NOT:INDEXED:FOREACH["; }
        public override string EndContext { get => "]::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is not indexed column in the table";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;
            if (table.Columns == null)
                throw new Exception($"The {nameof(TableModel.Columns)} are not set in {nameof(TableModel)}");
            if (table.Columns.Any(m => m == null))
                throw new Exception($"There is a null reference in the {nameof(TableModel.Columns)} from {nameof(TableModel)}");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var indexedColumns = table.Columns.Where(m => !m.IsIndexed);
            var eachIndexedcolumnResult = indexedColumns
                .Select(currentColumn => TemplateHandler.HandleTemplate(TrimedStringContext, DatabaseContextCopier.CopyWithOverride(databaseContext,currentColumn)));
            var result = string.Join(string.Empty, eachIndexedcolumnResult);
            return result;
        }
    }
}
