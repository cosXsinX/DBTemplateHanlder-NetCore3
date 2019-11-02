﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsAutoColumnALastAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:AUTO:FOREACH:CURRENT:IS:LAST:COLUMN(";
        public const String END_CONTEXT_WORD = "):::}";

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
            ColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(columnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (columnModel.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            List<ColumnModel> columnList = columnModel.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            ColumnModel currentLastAutoColumn = null;
            foreach(ColumnModel currentColumn in columnList)
            {
                if (currentColumn.IsAutoGeneratedValue)
                {
                    currentLastAutoColumn = currentColumn;
                }
            }
            if (currentLastAutoColumn == null) return "";
            if (!currentLastAutoColumn.Equals(columnModel)) return "";
            return HandleTrimedContext(TrimedStringContext);
        }

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }
}
