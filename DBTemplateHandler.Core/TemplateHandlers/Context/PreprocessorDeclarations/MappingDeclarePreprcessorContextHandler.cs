﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations
{
    public class MappingDeclarePreprcessorContextHandler : AbstactPreprocessorContextHandler
    {
        public MappingDeclarePreprcessorContextHandler(TemplateHandlerNew templateHandlerNew) 
            :base(templateHandlerNew)
        {
        }

        public IDatabaseModel DatabaseModel { get; set; }

        public override string StartContext => "{:TDB:PREPROCESSOR:MAPPING:DECLARE(";

        public override string EndContext => "):PREPROCESSOR:}";

        public override bool isStartContextAndEndContextAnEntireWord =>
            false;

        public override string ContextActionDescription => "Define the column types template code association mapping proxy for the processed template group context.";

        //TODO add in the mapping item first element all but something
        //\[->\(.*\)<-\][ \n\t]*<=>[ \n\t]*\[([ \n\t]*\[->\(.*\)<-\][ \n\t]*=>[ \n\t]*\[->\(.*\)<-\][ \n\t]*(,)?)+[ \n\t]*\]

        private const string mappingHeaderRegexPattern = "\\[->\\(.*\\)<-\\][ \\n\\t]*<=>[ \\n\\t]*\\[";
        Regex mappingHeaderRegex = new Regex(mappingHeaderRegexPattern);
        private const string mappingItemRegexPattern = "([ \\n\\t]*\\[->\\(.*\\)<-\\][ \\n\\t]*=>[ \\n\\t]*\\[->\\(.*\\)<-\\][ \\n\\t]*(,)?)";
        Regex mappingItemRegex = new Regex(mappingItemRegexPattern);
        Regex regex = new Regex(mappingHeaderRegexPattern + mappingItemRegexPattern +"+[ \\n\\t]*\\]");

        
        public override string PrepareProcessor(string TrimmedStringContext)
        {
            if (!regex.IsMatch(TrimmedStringContext)) 
                return $"{{:TDB:PREPROCESSOR:MAPPING:WARNING:NOT:HANDLED({TrimmedStringContext}):PREPROCESSOR:WARNING:NOT:HANDLED:}}";

            var headerMatches = mappingHeaderRegex.Matches(TrimmedStringContext);
            var headerMatch = headerMatches.FirstOrDefault();
            var headerAsString = ExtractMatch(headerMatch,TrimmedStringContext);
            string destinationTypeSetName = ToDestinationTypeSetName(headerAsString);

            var itemMatches = mappingItemRegex.Matches(TrimmedStringContext);
            var matches = itemMatches.Select(m => m);
            var itemAsStrings = matches.Select(m => ExtractMatch(m, TrimmedStringContext)).ToList();
            var items = itemAsStrings.Select(ToTypeMappingItem).Cast<ITypeMappingItem>().ToList();
            TemplateHandlerNew.OverwriteTypeMapping(new[] { 
                new TypeMapping() { 
                    SourceTypeSetName=DatabaseModel?.TypeSetName??"UnknownTypeSet" ,
                    DestinationTypeSetName = destinationTypeSetName,
                    TypeMappingItems = items 
                } 
            });
            return string.Empty;
        }

        private class TypeMapping : ITypeMapping
        {
            public string DestinationTypeSetName { get; set; }
            public string SourceTypeSetName { get; set; }
            public IList<ITypeMappingItem> TypeMappingItems { get; set; }
        }

        private class TypeMappingItem : ITypeMappingItem
        {
            public string DestinationType { get; set; }
            public string SourceType { get; set; }
        }


        private string ExtractMatch(Match match,string containing)
        {
            return containing.Substring(match.Index, match.Length);
        }

        private string ToDestinationTypeSetName(string headerString)
        {
            var header = Regex.Split(headerString, "\\)<-\\][ \\n\\t]*<=>[ \\n\\t]*\\[").First();
            header = header.Substring("[->(".Length);
            return header;
        }

        private TypeMappingItem ToTypeMappingItem(string itemAsString)
        {
            var splitted = Regex.Split(itemAsString, "\\)<-\\][ \\n\\t]*=>[ \\n\\t]*\\[->\\(").ToList();
            var sourceType = splitted[0].TrimStart().Substring(0, "[->(".Length);
            var destinationType = splitted[1].TrimEnd();
            if (destinationType.EndsWith(",")) destinationType.Substring(0, destinationType.Length - 1);
            destinationType.TrimEnd().Substring(0, destinationType.Length - ")<-]".Length);
            var result = new TypeMappingItem() { SourceType = sourceType, DestinationType = destinationType };
            return result;
        }
    }
}