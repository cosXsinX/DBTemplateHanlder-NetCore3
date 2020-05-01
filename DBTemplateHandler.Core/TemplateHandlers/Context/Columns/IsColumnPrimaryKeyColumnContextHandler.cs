using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnPrimaryKeyColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsColumnPrimaryKeyColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:PRIMARY(";
        public override string EndContext=> ")KEY:PRIMARY:::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current iteration column is a primary key column";


        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (columnModel.IsPrimaryKey)
            {
                return HandleTrimedContext(TrimedStringContext);
            }
            else return "";
        }


    }
}
