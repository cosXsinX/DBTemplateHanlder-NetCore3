﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachIndexedColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {

        public ForEachIndexedColumnTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:INDEXED:FOREACH["; }
        public override string EndContext { get => "]::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is indexed column in the table";


        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;
            if (table == null)
                throw new Exception($"The {nameof(table)} is not set");
            if (table.Columns == null)
                throw new Exception($"The {nameof(table.Columns)} are not set in {nameof(table)}");
            if (table.Columns.Any(m => m == null))
                throw new Exception($"There is a null reference in the {nameof(table.Columns)} from {nameof(table)}");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var indexedColumns = table.Columns.Where(m => m.IsIndexed);
            var eachIndexedcolumnResult = indexedColumns
                .Select(currentColumn => TemplateHandler.HandleTemplate(TrimedStringContext, DatabaseContextCopier.CopyWithOverride(databaseContext, currentColumn)));
            var result = string.Join(string.Empty, eachIndexedcolumnResult);
            return result;
        }
    }
}
