using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class NotPrimaryColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public NotPrimaryColumnIndexColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:INDEX"; }
        public override string EndContext { get => "::}"; }
        public override string ContextActionDescription => "Is replaced by the current not primary key column index in the current table not primary key column collection iterated";
        public override bool isStartContextAndEndContextAnEntireWord => true;

        public const int ZeroIndex = 0;
        public static readonly string ZeroIndexAsString = Convert.ToString(0);

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel column = databaseContext.Column;

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided StringContext :'{StringContext}' to the suited word '{Signature}'");
            if (databaseContext.Table == null)
                return ZeroIndexAsString;
            int currentIndex = 0;
            int currentAutoIndex = 0;
            IList<IColumnModel> columnList =
                    databaseContext.Table.Columns;
            for (currentIndex = 0; currentIndex < columnList.Count; currentIndex++)
            {
                IColumnModel currentColumn = columnList[currentIndex];
                if (currentColumn.IsPrimaryKey)
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
