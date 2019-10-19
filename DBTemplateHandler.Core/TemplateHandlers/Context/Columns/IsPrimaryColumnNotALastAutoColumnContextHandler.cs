﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsPrimaryColumnNotALastAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN(";
        public const String END_CONTEXT_WORD = "):::}";



        public override String getStartContextStringWrapper()
        {
            return START_CONTEXT_WORD;
        }


        public override String getEndContextStringWrapper()
        {
            return END_CONTEXT_WORD;
        }


        public override String processContext(String StringContext)
        {

            if (StringContext == null)
                throw new Exception("The provided StringContext is null");
            TableColumnDescriptionPOJO descriptionPojo = getAssociatedColumnDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedColumnDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (descriptionPojo.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            List<TableColumnDescriptionPOJO> columnList = descriptionPojo.ParentTable.get_ColumnsList();
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            TableColumnDescriptionPOJO currentLastAutoColumn = null;
            foreach (TableColumnDescriptionPOJO currentColumn in columnList)
            {
                if (currentColumn.is_PrimaryKey())
                {
                    currentLastAutoColumn = currentColumn;
                }
            }
            if (currentLastAutoColumn == null) return "";
            if (currentLastAutoColumn.Equals(descriptionPojo)) return "";
            return HandleTrimedContext(TrimedStringContext);
        }


        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }

}