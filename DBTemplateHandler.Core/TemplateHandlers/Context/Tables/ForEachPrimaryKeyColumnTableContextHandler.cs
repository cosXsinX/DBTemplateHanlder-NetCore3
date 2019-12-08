using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachPrimaryKeyColumnTableContextHandler : AbstractLoopColumnTableTemplateContextHandler
    {

        public ForEachPrimaryKeyColumnTableContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH["; }
        public override string EndContext { get => "]::}"; }
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is primary key column in the table";
        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            ITableModel descriptionPojo = TableModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(TableModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(ColumnModel currentColumn in descriptionPojo.Columns)
            {
                if (currentColumn.IsPrimaryKey)
                {
                    string treated =
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
