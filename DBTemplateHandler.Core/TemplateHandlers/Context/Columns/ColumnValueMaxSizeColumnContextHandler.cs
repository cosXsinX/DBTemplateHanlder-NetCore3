using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnValueMaxSizeColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public ColumnValueMaxSizeColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:MAX:SIZE";
        public override string EndContext => "::}";
        public override string ContextActionDescription => "Is replaced by the current column value max/length size";

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '" + (Signature) + "'");
            return $"{columnModel.ValueMaxSize}";
        }


        public override bool isStartContextAndEndContextAnEntireWord => true;

    }
}
