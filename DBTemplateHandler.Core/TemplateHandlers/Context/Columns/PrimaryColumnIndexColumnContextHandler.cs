using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class PrimaryColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public PrimaryColumnIndexColumnContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:INDEX"; }
        public override string EndContext { get => "::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => true;
        public override string ContextActionDescription => "Is replaced by the current primary key column index in the current table primary key column collection iterated";

        private const int ZeroIndex = 0;
        private readonly static string ZeroIndexAsString = Convert.ToString(ZeroIndex);
        public override string processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided StringContext :'{StringContext}' to the suited word '{Signature}'");
            if (columnModel.ParentTable == null)
                return ZeroIndexAsString;
            int currentIndex = ZeroIndex;
            int currentAutoIndex = ZeroIndex;
            IList<IColumnModel> columnList =
            columnModel.ParentTable.Columns;
            for (currentIndex = 0; currentIndex < columnList.Count; currentIndex++)
            {
                IColumnModel currentColumn = columnList[currentIndex];
                if (!currentColumn.IsPrimaryKey)
                {
                    if (currentColumn.Equals(columnModel))
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
