using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsPrimaryColumnAFirstPrimaryColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsPrimaryColumnAFirstPrimaryColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN("; }
        public override string EndContext { get => "):::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the first column from the iterated primary key column collection";

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

            foreach (IColumnModel currentColumn in columnList)
            {
                if (currentColumn.IsPrimaryKey)
                {
                    if (currentColumn.Equals(descriptionPojo))
                    {
                        return HandleTrimedContext(TrimedStringContext);
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }

    }
}
