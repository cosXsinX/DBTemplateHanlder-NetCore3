using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Database
{
    public class DatabaseNameDatabaseContextHandler : AbstractDatabaseTemplateContextHandler
    {
        public DatabaseNameDatabaseContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:CURRENT:NAME"; }
        public override string EndContext { get => "::}"; }
        public override string ContextActionDescription => "Is replaced by the current database name";

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IDatabaseModel descriptionPojo = DatabaseModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(DatabaseModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (TrimedStringContext != "")
                throw new Exception($"There is a problem with the provided StringContext :'{StringContext}' to the suited word '{Signature}'");
            return descriptionPojo.Name;
        }

        public override bool isStartContextAndEndContextAnEntireWord => true;
    }
}
