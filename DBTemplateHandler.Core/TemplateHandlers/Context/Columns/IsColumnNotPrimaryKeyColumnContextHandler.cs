using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnNotPrimaryKeyColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:PRIMARY(";
        public const String END_CONTEXT_WORD = ")KEY:NOT:PRIMARY:::}";



        public override string StartContext
        {
            get => START_CONTEXT_WORD;
        }

        public override string EndContext
        {
            get => END_CONTEXT_WORD;
        }

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!columnModel.IsPrimaryKey)
            {
                return HandleTrimedContext(TrimedStringContext);
            }
            else return "";
        }


        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }
}
