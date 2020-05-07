﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsAutoColumnAFirstAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsAutoColumnAFirstAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:FIRST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the first column from the iterated auto generated value column collection";

        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { Column = ColumnModel });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = databaseContext.Column;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");
            if (columnModel.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = columnModel.ParentTable.Columns;
            if (columnList == null || !columnList.Any())
                throw new Exception("The provided column's parent table has no column associated to");

            if (columnModel.IsAutoGeneratedValue == false) return string.Empty;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var indexedColumn = columnList.FirstOrDefault(currentColumn => currentColumn.IsAutoGeneratedValue);
            if (indexedColumn == default(IColumnModel)) return string.Empty;
            if (indexedColumn.Equals(columnModel)) return HandleTrimedContext(TrimedStringContext);
            return string.Empty;
        }
    }
}
