﻿using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class TableTemplateHandlerInputModel
    {
        public IList<TemplateModel> templateModels {get;set;}
        public TableModel databaseDescriptor { get; set; }
    }
}
