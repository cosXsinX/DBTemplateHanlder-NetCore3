using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class DatabaseTemplateHandlerInputModel
    {
        public IList<TemplateModel> templateModels {get;set;}
        public DatabaseModel databaseDescriptor { get; set; }
    }
}
