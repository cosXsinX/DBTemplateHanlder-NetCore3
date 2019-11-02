using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class DatabaseTemplateHandlerInputModel
    {
        public IList<TemplateModel> TemplateModels {get;set;}
        public DatabaseModel DatabaseModel { get; set; }
    }
}
