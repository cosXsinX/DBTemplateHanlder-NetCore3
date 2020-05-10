using System;
using System.Collections.Generic;
using System.Text;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Constraints
{
    public class ConstraintNameConstraintContextHandler : AbstractConstraintTemplateContextHandler
    {
        public ConstraintNameConstraintContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:CONSTRAINT:CURRENT:NAME";

        public override string EndContext => "::}";

        public override bool isStartContextAndEndContextAnEntireWord => true;

        public override string ContextActionDescription => "Is replaced by the current constraint name from the iteration";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            var constraintModel = databaseContext.ForeignKeyConstraint;
            string TrimmedSringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimmedSringContext.Equals(string.Empty))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{Signature}'");
            return constraintModel.ConstraintName;
        }
    }
}
