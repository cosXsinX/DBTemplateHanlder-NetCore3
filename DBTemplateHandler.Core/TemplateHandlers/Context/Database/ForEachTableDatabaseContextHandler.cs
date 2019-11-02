using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public class ForEachTableDatabaseContextHandler : AbstractDatabaseTemplateContextHandler
    {
        public const String START_CONTEXT_WORD = "{:TDB:TABLE:FOREACH[";
        public const String END_CONTEXT_WORD = "]::}";
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            DatabaseModel descriptionPojo = DatabaseModel;
            if (descriptionPojo == null)
                throw new Exception($"The {typeof(DatabaseModel).Name} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(TableModel currentColumn in descriptionPojo.Tables)
            {
                String treated = TemplateHandlerNew.
                        HandleTableTemplate(TrimedStringContext, currentColumn);
                treated = TemplateHandlerNew.HandleFunctionTemplate(treated, descriptionPojo, currentColumn, null);
                stringBuilder.Append(treated);
            }
            return stringBuilder.ToString();
        }

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }
}
