﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class NotAutoColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public NotAutoColumnIndexColumnContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:AUTO:FOREACH:CURRENT:INDEX";
        public override string EndContext => "::}";
        public override bool isStartContextAndEndContextAnEntireWord => true;
        public override string ContextActionDescription => "Is replaced by the current not auto generated value column index in the current table not auto generated value column collection iterated";

        public const int ZeroIndex = 0;
        public readonly static string ZeroIndexAsString = Convert.ToString(ZeroIndex);
        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided StringContext :'{StringContext}' to the suited word '{Signature}'");
            if (columnModel.ParentTable == null)
                return ZeroIndexAsString;
            int currentIndex = ZeroIndex;
            int currentAutoIndex = ZeroIndex;
            IList<IColumnModel> columnList =
                    columnModel.ParentTable.Columns;
            for (currentIndex = 0; currentIndex < columnList.Count; currentIndex++)
            {
                IColumnModel currentColumn = columnList[currentIndex];
                if (!currentColumn.IsAutoGeneratedValue)
                {
                    if (currentColumn.Equals(columnModel))
                    {
                        return Convert.ToString(currentAutoIndex);
                    }
                    currentAutoIndex++;
                }
            }
            return ZeroIndexAsString;
        }
    }
}
