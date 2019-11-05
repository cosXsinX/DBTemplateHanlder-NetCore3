using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class TemplateModel : ITemplateModel
    {
        public string TemplatedFilePath { get; set; }
        public string TemplatedFileContent { get; set; }
    }
}
