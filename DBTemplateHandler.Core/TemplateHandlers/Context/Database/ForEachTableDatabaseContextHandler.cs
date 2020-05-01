using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public class ForEachTableDatabaseContextHandler : AbstractLoopTableDatabaseTemplateContextHandler
    {
        public ForEachTableDatabaseContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew){}

        public override string StartContext { get => "{:TDB:TABLE:FOREACH["; }
        public override string EndContext { get => "]::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is table in the database";

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IDatabaseModel descriptionPojo = DatabaseModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(DatabaseModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(ITableModel currentColumn in descriptionPojo.Tables)
            {
                string treated = TemplateHandler.
                        HandleTableTemplate(TrimedStringContext, currentColumn);
                treated = TemplateHandler.HandleFunctionTemplate(treated, descriptionPojo, currentColumn, null);
                stringBuilder.Append(treated);
            }
            return stringBuilder.ToString();
        }


    }
}
