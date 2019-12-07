﻿using System;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public abstract class AbstractTableTemplateContextHandler : AbstractTemplateContextHandler, ITableTemplateContextHandler
    {
        public ITableModel TableModel { get; set; }

        public override string HandleTrimedContext(string StringTrimedContext)
        {
		    if(StringTrimedContext == null) return null;
		    ITableModel table = TableModel;
		    if(table == null) return StringTrimedContext;
		    IDatabaseModel database = table.ParentDatabase;
		    return TemplateHandlerNew.
                    HandleTemplate(StringTrimedContext, database,
                            table, null );
	    }
    }
}