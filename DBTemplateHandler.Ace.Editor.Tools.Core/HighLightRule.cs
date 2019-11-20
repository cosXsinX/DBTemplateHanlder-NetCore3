using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Ace.Editor.Tools.Core
{
    public class HighLightRule
    {
        public string Token { get; set; }
        public string Regex { get; set; }
        public string Next { get; set; }
    }

    public class HighLightRuleWithCurrent : HighLightRule
    {
        public string Current { get; set; }
    }
}
