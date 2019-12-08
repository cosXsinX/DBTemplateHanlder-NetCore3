using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class TableNameTableContextHandler : AbstractTableTemplateContextHandler
    {

        public TableNameTableContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:CURRENT:NAME"; }
        public override string EndContext { get => "::}"; }

        public override bool isStartContextAndEndContextAnEntireWord => true;
        public override string ContextActionDescription => "Is replaced by the current table name";
        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel table = TableModel;
            if (table == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{Signature}'");
            return table.Name;
        }

    }
}
