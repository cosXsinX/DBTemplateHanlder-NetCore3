﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotPrimaryColumnAFirstAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsNotPrimaryColumnAFirstAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the first column from the iterated not primary key column collection";

        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { Column = ColumnModel });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel column = ColumnModel;
            if (column == null)
                throw new Exception($" The {nameof(ColumnModel)} is not set");
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (column.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = column.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");

            foreach (IColumnModel currentColumn in columnList)
            {
                if (!currentColumn.IsPrimaryKey)
                {
                    if (currentColumn.Equals(column))
                    {
                        return HandleTrimedContext(TrimedStringContext);
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }
    }
}
