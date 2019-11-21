using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Ace.Editor.Tools.Core
{
    public class HighLightRule
    {
        public string token { get; set; }
        public string regex { get; set; }
        public string next { get; set; }
    }

    public class HighLightRuleWithCurrent : HighLightRule
    {
        public string Current { get; set; }
    }
}
