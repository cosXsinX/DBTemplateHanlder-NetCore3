using DBTemplateHandler.Core.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.ModelImplementation
{
    public class TemplateModelForTest : ITemplateModel
    {
        public string TemplatedFileContent { get; set; }
        public string TemplatedFilePath { get; set; }
    }
}
