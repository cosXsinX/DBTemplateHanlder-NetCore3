using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class ForEachConstraintTableContextHandler : AbstractLoopConstraintTableTemplateContextHandler
    {
        public ForEachConstraintTableContextHandler(ITemplateHandler templateHandler): base(templateHandler) { }
        public override string StartContext => "{:TDB:TABLE:CONSTRAINT:FOREACH[";

        public override string EndContext => "]::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the intern context as many time as there is constraint on the table";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;
            string trimedStringContext = TrimContextFromContextWrapper(StringContext);
            var constraints = table.ForeignKeyConstraints;
            if (constraints == null) throw new ArgumentNullException(nameof(table.ForeignKeyConstraints));
            var result = string.Join(string.Empty,
                constraints.Select(constraint =>
                    TemplateHandler.HandleTemplate(trimedStringContext, DatabaseContextCopier.CopyWithOverride(databaseContext, constraint))));
            return result;
        }
    }
}
