﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class WhenHasAutoGeneratedValueColumnTableContextHandler : AbstractTableTemplateContextHandler
    {
        public WhenHasAutoGeneratedValueColumnTableContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:CURRENT:WHEN:HAS:AUTO(";

        public override string EndContext => ")::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the processed content value when the current table has auto generated values";

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel table = TableModel;
            if (table == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            
            if (!(table?.Columns ?? new List<IColumnModel>()).Any(m => m.IsAutoGeneratedValue))
                return String.Empty;
            
            var result = TemplateHandlerNew.
                            HandleFunctionTemplate
                                            (TrimedStringContext, TableModel.ParentDatabase,TableModel, null);
            return result;
        }
    }
}
