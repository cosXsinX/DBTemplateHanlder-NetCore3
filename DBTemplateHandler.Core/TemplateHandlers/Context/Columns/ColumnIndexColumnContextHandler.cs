using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnIndexColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:INDEX";
        public override string EndContext => "::}";
        public override bool isStartContextAndEndContextAnEntireWord => true;

        private readonly static string ZeroAsString = Convert.ToString(0);
        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception(
                    $"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{string.Concat(StartContext,EndContext)}'");
            if (columnModel.ParentTable == null)
                return ZeroAsString;
            IList<IColumnModel> columnList =
                    columnModel.ParentTable.Columns;
            return Convert.ToString(columnList.IndexOf(columnModel));
        }

        
    }
}
