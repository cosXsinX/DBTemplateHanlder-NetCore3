﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotAutoColumnNotAFirstAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsNotAutoColumnNotAFirstAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:IS:NOT:FIRST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is not the first column from the iterated not auto generated value column collection";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel column = databaseContext.Column;
            if (column == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (databaseContext.Table == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = databaseContext.Table.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");

            foreach (IColumnModel currentColumn in columnList)
            {
                if (!currentColumn.IsAutoGeneratedValue)
                {
                    if (!currentColumn.Equals(column))
                    {
                        return HandleTrimedContext(TrimedStringContext,databaseContext);
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
