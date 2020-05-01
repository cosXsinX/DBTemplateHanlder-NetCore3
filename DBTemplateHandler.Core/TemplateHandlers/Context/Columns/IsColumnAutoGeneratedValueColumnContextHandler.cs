﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnAutoGeneratedValueColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsColumnAutoGeneratedValueColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:AUTO(";
        public override string EndContext => ")KEY:AUTO:::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current iteration column is an auto generated value column";


        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {StringContext} is null");
            IColumnModel columnModel = ColumnModel;
            if (columnModel == null)
                throw new Exception($"The {ColumnModel} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (columnModel.IsAutoGeneratedValue)
            {
                return HandleTrimedContext(TrimedStringContext);
            }
            else return "";
        }


    }
}
