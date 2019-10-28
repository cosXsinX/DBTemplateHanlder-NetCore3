using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public class DatabaseNameDatabaseContextHandler : AbstractDatabaseTemplateContextHandler
    {
        private const String START_CONTEXT_WORD = "{:TDB:CURRENT:NAME";
        private const String END_CONTEXT_WORD = "::}";

        public readonly static String TEMPLATE_TABLE_WORD = START_CONTEXT_WORD + END_CONTEXT_WORD;
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception("The provided StringContext is null");
            DatabaseDescriptor descriptionPojo = getAssociatedDatabaseDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedDatabaseDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (TrimedStringContext != "")
                throw new Exception("There is a problem with the provided StringContext :'" + StringContext + "' to the suited word '" + (START_CONTEXT_WORD + END_CONTEXT_WORD) + "'");
            return descriptionPojo.Name;
        }

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return true;
        }
    }
}
