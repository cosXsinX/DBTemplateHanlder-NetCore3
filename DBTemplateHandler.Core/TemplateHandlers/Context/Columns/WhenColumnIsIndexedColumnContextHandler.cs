﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class WhenColumnIsIndexedColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public WhenColumnIsIndexedColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:WHEN:IS:INDEXED(";

        public override string EndContext => ")::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the inner content when current column is indexed, instead it will be replaced by an empty string";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel columnModel = databaseContext.Column;
            if (!columnModel.IsIndexed) return string.Empty;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            return HandleTrimedContext(TrimedStringContext,databaseContext);
        }
    }
}
