using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class NotNullColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public NotNullColumnIndexColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:NOT:NULL:CURRENT:INDEX"; }
        public override string EndContext { get => "::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => true;
        public override string ContextActionDescription => "Is replaced by the current not nullable value column index in the current table not nullable value column collection iterated";

        private const int ZeroIndex = 0;
        private readonly static string ZeroIndexAsString = Convert.ToString(ZeroIndex);

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel column = databaseContext.Column;

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided StringContext :'{StringContext}' to the suited word '{Signature}'");
            if (databaseContext.Table == null)
                return ZeroIndexAsString;
            int currentIndex = ZeroIndex;
            int currentAutoIndex = ZeroIndex;
            IList<IColumnModel> columnList =
                    databaseContext.Table.Columns;
            for (currentIndex = 0; currentIndex < columnList.Count; currentIndex++)
            {
                IColumnModel currentColumn = columnList[currentIndex];
                if (currentColumn.IsNotNull)
                {
                    if (currentColumn.Equals(column))
                    {
                        return Convert.ToString(currentAutoIndex);
                    }
                    currentAutoIndex++;
                }
            }
            return ZeroIndexAsString;
        }
    }
}
