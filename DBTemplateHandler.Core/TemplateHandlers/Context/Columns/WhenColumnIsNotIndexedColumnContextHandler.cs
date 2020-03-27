using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class WhenColumnIsNotIndexedColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public WhenColumnIsNotIndexedColumnContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:WHEN:IS:NOT:INDEXED(";

        public override string EndContext => ")::}";

        public override bool isStartContextAndEndContextAnEntireWord => false;

        public override string ContextActionDescription => "Is replaced by the inner content when current column is not indexed, instead it will be replaced by an empty string";

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            if (columnModel.IsIndexed) return string.Empty;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            return HandleTrimedContext(TrimedStringContext);
        }
    }
}
