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

        public override String getStartContextStringWrapper()
        {
            return START_CONTEXT_WORD;
        }


        public override String getEndContextStringWrapper()
        {
            return END_CONTEXT_WORD;
        }

        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception("The provided StringContext is null");
            DatabaseDescriptor descriptionPojo = getAssociatedDatabaseDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedDatabaseDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(TableDescriptor currentColumn in descriptionPojo.Tables)
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
