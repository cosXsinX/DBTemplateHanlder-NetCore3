using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class DatabaseTemplateHandlerInputModel : IDatabaseTemplateHandlerInputModel
    {
        public IList<ITemplateModel> TemplateModels { get; set; }
        public IDatabaseModel DatabaseModel { get; set; }
    }
}
