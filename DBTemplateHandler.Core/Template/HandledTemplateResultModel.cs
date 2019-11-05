using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Template
{
    public class HandledTemplateResultModel:IHandledTemplateResultModel
    {
        public string Path { get; set; }
        public string Content { get; set; }
    }
}
