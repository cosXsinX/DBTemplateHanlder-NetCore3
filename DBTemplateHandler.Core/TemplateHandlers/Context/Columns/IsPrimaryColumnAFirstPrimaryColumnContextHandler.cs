﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsPrimaryColumnAFirstPrimaryColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsPrimaryColumnAFirstPrimaryColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN("; }
        public override string EndContext { get => "):::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the first column from the iterated primary key column collection";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel column = databaseContext.Column;

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (databaseContext.Table == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = databaseContext.Table.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");

            foreach (IColumnModel currentColumn in columnList)
            {
                if (currentColumn.IsPrimaryKey)
                {
                    if (currentColumn.Equals(column))
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
