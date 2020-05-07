using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {
        public ForEachColumnTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext  => "{:TDB:TABLE:COLUMN:FOREACH["; 
        public override string EndContext => "]::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is column in the table";
        public override string processContext(string StringContext)
        {
            return ProcessContext(StringContext, new ProcessorDatabaseContext() { Table = TableModel });
        }

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel descriptionPojo = databaseContext.Table;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (IColumnModel currentColumn in descriptionPojo.Columns)
            {
                string treated = TemplateHandler.HandleTableColumnTemplate(TrimedStringContext, currentColumn);
                treated = TemplateHandler.HandleFunctionTemplate(treated, descriptionPojo.ParentDatabase, descriptionPojo, currentColumn, null);
                stringBuilder.Append(treated);
            }
            return stringBuilder.ToString();
        }
    }
}
