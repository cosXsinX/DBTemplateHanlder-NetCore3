using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class TemplateModel : ITemplateModel
    {
        public string TemplatedFilePath { get; set; }
        public string TemplatedFileContent { get; set; }

        public ITemplateModel Copy()
        {
            var result = new TemplateModel();
            result.TemplatedFilePath = result.TemplatedFilePath;
            result.TemplatedFileContent = result.TemplatedFileContent;
            return result;
        }
    }
}
