using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace DBTemplateHandler.Ace.Editor.Tools.Core
{
    public class HighLightRulesGenerator
    {
        private readonly InputModelHandler handler;
        private readonly TemplateContextHandlerRegister templateContextHandlerRegister;

        public HighLightRulesGenerator(
            InputModelHandler handler,
            TemplateContextHandlerRegister templateContextHandlerRegister)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            if (templateContextHandlerRegister == null) throw new ArgumentNullException(nameof(templateContextHandlerRegister));
            this.handler = handler;
            this.templateContextHandlerRegister = templateContextHandlerRegister;
        }

        public Dictionary<string,List<HighLightRule>> GenerateHighLighRule()
        {
            var handlers = templateContextHandlerRegister.GetHanlders<ITemplateContextHandler>().ToList();
            var startContextes = handlers.Select(handler => handler.StartContext).Distinct().ToList(); ;
            var startContextHighlighRules = startContextes.Select(m => new HighLightRuleWithCurrent()
            {
                Current = "start",
                Token = "keyword",
                Regex = Regex.Escape(m),
                Next = "end-context",
            }).ToList();

            var endContextes = handlers.Select(m => m.EndContext).Distinct().ToList();
            var endContextHighlightRules = endContextes.Select(m => new HighLightRuleWithCurrent()
            {
                Current = "end-context",
                Token = "keyword",
                Regex = Regex.Escape(m),
                Next = "start"
            }).ToList();
            var result = startContextHighlighRules.Concat(endContextHighlightRules).ToList();
            return result.GroupBy(m => m.Current, m => ToHighLightRule(m)).ToDictionary(m => m.Key, m => m.ToList()); ;
        }

        public string GenerateHighlightRuleJson()
        {
            var encoded = GenerateHighLighRule();
            return Json.Encode(encoded);
        }

        private HighLightRule ToHighLightRule(HighLightRuleWithCurrent converted)
        {
            var result = new HighLightRule();
            result.Next = converted.Next;
            result.Regex = converted.Regex;
            result.Token = converted.Token;
            return result;
        }
        
        public string PopulateHighlightRuleContentWithPlaceHolder(string contentWithPlaceHolder)
        {
            const string highLightRulePlaceHolder = "{-->highlight_rule<--}";
            string highLightRuleJson = GenerateHighlightRuleJson();
            var result = contentWithPlaceHolder.Replace(highLightRulePlaceHolder, highLightRuleJson);
            return result;
        }

    }
}
