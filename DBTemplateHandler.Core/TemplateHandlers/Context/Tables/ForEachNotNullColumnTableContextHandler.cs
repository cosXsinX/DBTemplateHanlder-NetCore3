using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachNotNullColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {
        public ForEachNotNullColumnTableContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }

        public override string StartContext { get => "{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH["; }
        public override string EndContext { get => "]::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is not nullable value column in the table";
        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel descriptionPojo = TableModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ColumnModel currentColumn in descriptionPojo.Columns)
            {
                if (currentColumn.IsNotNull)
                {
                    String treated =
                            TemplateHandlerNew.HandleTableColumnTemplate
                                (TrimedStringContext, currentColumn);
                    treated = TemplateHandlerNew.
                            HandleFunctionTemplate
                                            (treated, descriptionPojo.ParentDatabase,
                                                    descriptionPojo, currentColumn);
                    stringBuilder.Append(treated);
                }
            }
            return stringBuilder.ToString();
        }

    }
}
