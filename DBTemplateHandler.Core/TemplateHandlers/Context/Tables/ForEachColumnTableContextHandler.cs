using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {
        public ForEachColumnTableContextHandler(ITemplateHandler templateHandler) : base(templateHandler) { }
        public override string StartContext  => "{:TDB:TABLE:COLUMN:FOREACH["; 
        public override string EndContext => "]::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is column in the table";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var columns = table.Columns;
            if(columns == null) throw new ArgumentNullException(nameof(table.Columns));
            var result = string.Join(string.Empty, 
                columns.Select(currentColumn => 
                    TemplateHandler.HandleTemplate(TrimedStringContext, 
                        DatabaseContextCopier.CopyWithOverride(databaseContext, currentColumn))));
            return result;
        }
    }
}
