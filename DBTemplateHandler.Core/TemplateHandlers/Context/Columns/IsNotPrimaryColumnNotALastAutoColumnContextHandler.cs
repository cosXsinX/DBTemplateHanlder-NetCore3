using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotPrimaryColumnNotALastAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsNotPrimaryColumnNotALastAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN("; }
        public override string EndContext { get => "):::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is not the last column from the iterated not primary key column collection";


        public override string processContext(string StringContext)
        {

            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel descriptionPojo = ColumnModel;
            if (descriptionPojo == null)
                throw new Exception($" The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (descriptionPojo.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = descriptionPojo.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            IColumnModel currentLastPrimaryColumn = null;
            foreach (IColumnModel currentColumn in columnList)
            {
                if (!currentColumn.IsPrimaryKey)
                {
                    currentLastPrimaryColumn = currentColumn;
                }
            }
            if (currentLastPrimaryColumn == null) return "";
            if (currentLastPrimaryColumn.Equals(descriptionPojo)) return "";
            return HandleTrimedContext(TrimedStringContext);
        }


    }
}
