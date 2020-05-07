﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class NotNullColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public NotNullColumnIndexColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:NOT:NULL:CURRENT:INDEX"; }
        public override string EndContext { get => "::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => true;
        public override string ContextActionDescription => "Is replaced by the current not nullable value column index in the current table not nullable value column collection iterated";

        private const int ZeroIndex = 0;
        private readonly static string ZeroIndexAsString = Convert.ToString(ZeroIndex);
        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { Column = ColumnModel });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
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
                if (currentColumn.IsNotNull)
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
