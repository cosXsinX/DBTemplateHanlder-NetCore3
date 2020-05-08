using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Linq;
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

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IDatabaseModel descriptionPojo = databaseContext.Database;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            string result = string.Join(string.Empty, descriptionPojo.Tables.Select(currentTable =>
                TemplateHandler.HandleTemplate(TrimedStringContext, DatabaseContextCopier.CopyWithOverride(databaseContext, currentTable))));
            return result;
        }
    }
}
