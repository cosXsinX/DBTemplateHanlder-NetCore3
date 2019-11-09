using DBTemplateHandler.Core.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Persistance
{
    [Serializable]
    public class TemplateGroup
    {
        public string Name { get; set; }
        public IList<ITemplateModel> templateModels { get; set; }
    }
}
