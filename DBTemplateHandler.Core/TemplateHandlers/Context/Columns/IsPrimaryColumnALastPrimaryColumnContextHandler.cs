﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsPrimaryColumnALastPrimaryColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN("; }
        public override string EndContext { get => "):::}"; }

        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the last column from the iterated primary key column collection";

        public override string processContext(string StringContext)
        {

            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel descriptionPojo = ColumnModel;
            if (descriptionPojo == null)
                throw new Exception($" The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (descriptionPojo.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = descriptionPojo.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            IColumnModel currentLastAutoColumn = null;
            foreach(IColumnModel currentColumn in columnList)
            {
                if (currentColumn.IsPrimaryKey)
                {
                    currentLastAutoColumn = currentColumn;
                }
            }
            if (currentLastAutoColumn == null) return "";
            if (!currentLastAutoColumn.Equals(descriptionPojo)) return "";
            return HandleTrimedContext(TrimedStringContext);
        }


    }
}