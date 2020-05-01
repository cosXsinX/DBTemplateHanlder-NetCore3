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
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel descriptionPojo = TableModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (IColumnModel currentColumn in descriptionPojo.Columns)
            {
                string treated = TemplateHandler.HandleTableColumnTemplate(TrimedStringContext, currentColumn);
                treated = TemplateHandler.HandleFunctionTemplate(treated, descriptionPojo.ParentDatabase, descriptionPojo, currentColumn);
                stringBuilder.Append(treated);
            }
            return stringBuilder.ToString();
        }

    }
}
