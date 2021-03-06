﻿using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DBTemplateHandler.Ace.Editor.Tools.Core
{
    public class HighLightRulesGenerator
    {
        private readonly TemplateContextHandlerRegister templateContextHandlerRegister;

        public HighLightRulesGenerator(
            TemplateContextHandlerRegister templateContextHandlerRegister)
        {
            if (templateContextHandlerRegister == null) throw new ArgumentNullException(nameof(templateContextHandlerRegister));
            this.templateContextHandlerRegister = templateContextHandlerRegister;
        }

        public Dictionary<string,List<HighLightRule>> GenerateHighLighRule()
        {
            var handlers = templateContextHandlerRegister.GetHanlders<ITemplateContextHandler>().ToList();
            var startContextes = handlers.Select(handler => handler.StartContext).Distinct().ToList(); ;
            var startContextHighlighRules = startContextes.Select(m => new HighLightRuleWithCurrent()
            {
                Current = "start",
                token = "keyword",
                regex = Regex.Escape(m),
                next = "end-context",
            }).ToList();

            var endContextes = handlers.Select(m => m.EndContext).Distinct().ToList();
            var endContextHighlightRules = endContextes.Select(m => new HighLightRuleWithCurrent()
            {
                Current = "end-context",
                token = "keyword",
                regex = Regex.Escape(m),
                next = "start"
            }).ToList();
            var result = startContextHighlighRules.Concat(endContextHighlightRules).ToList();
            return result.GroupBy(m => m.Current, m => ToHighLightRule(m)).ToDictionary(m => m.Key, m => m.ToList()); ;
        }

        public string GenerateHighlightRuleJson()
        {
            var encoded = GenerateHighLighRule();
            return JsonSerializer.Serialize(encoded);
        }

        private HighLightRule ToHighLightRule(HighLightRuleWithCurrent converted)
        {
            var result = new HighLightRule();
            result.next = converted.next;
            result.regex = converted.regex;
            result.token = converted.token;
            return result;
        }
        
        public string PopulateHighlightRuleContentWithPlaceHolder(string contentWithPlaceHolder)
        {
            const string highLightRulePlaceHolder = "{-->highlight_rule<--}";
            string highLightRuleJson = GenerateHighlightRuleJson();
            var result = contentWithPlaceHolder.Replace(highLightRulePlaceHolder, highLightRuleJson);
            return result;
        }
        public string PopulateSemantic(string contentWithPlaceHolder)
        {
            const string highLightRulePlaceHolder = "{-->semantic<--}";
            string highLightRuleJson = GetSemanticRulesJson();
            var result = contentWithPlaceHolder.Replace(highLightRulePlaceHolder, highLightRuleJson);
            return result;
        }



        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public FileModel GetDbTemplateAceMode()
        {
            string fileName = "mode-dbtemplate.js";
            var highlightRulesWithPlaceHolders = Path.Combine(AssemblyDirectory, "Ressources", fileName);
            var fileContent = File.ReadAllText(highlightRulesWithPlaceHolders);
            var populatedFileContent = PopulateHighlightRuleContentWithPlaceHolder(fileContent);
            populatedFileContent = PopulateSemantic(populatedFileContent);
            return new FileModel()
            {
                FileName = fileName,
                Content = populatedFileContent
            };
        }

        

        private string GetSemanticRulesJson()
        {
            var handlers = templateContextHandlerRegister.GetHanlders<ITemplateContextHandler>().ToList();
            var rules = handlers.Select(handler =>
                new
                {
                    startContext = handler.StartContext,
                    endContext = handler.EndContext,
                    handler = handler.GetType().Name,
                    category = ToCategory(handler),
                    isStartContextAndEndContextAnEntireWord = handler.isStartContextAndEndContextAnEntireWord
                }).ToList();
            var fileContent = JsonSerializer.Serialize(rules);
            return fileContent;
        }

        public string ToCategory(ITemplateContextHandler handler)
        {
            if (handler is AbstractLoopTableDatabaseTemplateContextHandler) return "table-loop";
            if (handler is AbstractLoopColumnTableTemplateContextHandler) return "column-loop";
            if (handler is AbstractFunctionTemplateContextHandler) return "function";
            if (handler is AbstractDatabaseTemplateContextHandler) return "database";
            if (handler is AbstractTableTemplateContextHandler) return "table";
            if (handler is AbstractColumnTemplateContextHandler) return "column";
            return "unknown";
        }
    }
}
