using DBTemplateHandler.Core.Database;
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
        private readonly string sourceTypeStartContext;
        private readonly string sourceTypeEndContext;
        private readonly string sourceDestinationSeparator;
        private readonly string destinationTypeStartContext;
        private readonly string destinationTypeEndContext;

        public MappingDeclarePreprcessorContextHandler(ITemplateHandler templateHandlerNew) 
            :base(templateHandlerNew)
        {
            sourceTypeStartContext = "[->(";
            var sourceTypeStartContextPattern = Regex.Escape(sourceTypeStartContext);
            sourceTypeEndContext = ")<-]";
            var sourceTypeEndContextPattern = Regex.Escape(sourceTypeEndContext);

            sourceDestinationSeparator = "=>";
            var sourceDestinationSeparatorPattern = Regex.Escape(sourceDestinationSeparator);

            destinationTypeStartContext = "[->(";
            var destinationTypeStartContextPattern = Regex.Escape(destinationTypeStartContext);
            destinationTypeEndContext = ")<-]";
            var destinationTypeEndContextPattern = Regex.Escape(destinationTypeEndContext);

            mappingHeaderRegexPattern = "\\[->\\(.*\\)<-\\]\\s*<=>\\s*\\[";
            mappingHeaderRegex = new Regex(mappingHeaderRegexPattern);
            mappingItemRegexPattern = $"(\\s*{sourceTypeStartContextPattern}.*{sourceTypeEndContextPattern}\\s*{sourceDestinationSeparatorPattern}\\s*{destinationTypeStartContextPattern}.*{destinationTypeEndContextPattern}\\s*(,)?)";
            mappingItemRegex = new Regex(mappingItemRegexPattern);
            regex = new Regex(mappingHeaderRegexPattern + mappingItemRegexPattern + "+\\s*\\]");
        }

        public IDatabaseModel DatabaseModel { get; set; }

        public override string StartContext => "{:TDB:PREPROCESSOR:MAPPING:DECLARE(";

        public override string EndContext => "):PREPROCESSOR:}";

        public override bool isStartContextAndEndContextAnEntireWord =>
            false;

        public override string ContextActionDescription => "Define the column types template code association mapping proxy for the processed template group context.";

        //TODO add in the mapping item first element all but something
        //\[->\(.*\)<-\]\s*<=>\s*\[(\s*\[->\(.*\)<-\]\s*=>\s*\[->\(.*\)<-\]\s*(,)?)+\s*\]

        private readonly string mappingHeaderRegexPattern;
        private readonly Regex mappingHeaderRegex;
        private readonly string mappingItemRegexPattern;
        private readonly Regex mappingItemRegex;
        private readonly Regex regex;
        
        public override string PrepareProcessor(string TrimmedStringContext)
        {
            if (!regex.IsMatch(TrimmedStringContext)) 
                return $"{{:TDB:PREPROCESSOR:MAPPING:WARNING:NOT:HANDLED({TrimmedStringContext}):PREPROCESSOR:WARNING:NOT:HANDLED:}}";

            var headerMatches = mappingHeaderRegex.Matches(TrimmedStringContext);
            var headerMatch = headerMatches.FirstOrDefault();
            var headerAsString = ExtractMatch(headerMatch,TrimmedStringContext);
            string destinationTypeSetName = ToDestinationTypeSetName(headerAsString);

            var trimmedContextWithoutHeader = TrimmedStringContext.Substring(headerMatch.Index + headerMatch.Length);
            trimmedContextWithoutHeader = trimmedContextWithoutHeader.Substring(0, trimmedContextWithoutHeader.Length - 1);
            var items = ToTypeMappingItems(trimmedContextWithoutHeader).ToList();
            
            TemplateHandler.OverwriteTypeMapping(new[] { 
                new TypeMapping() { 
                    SourceTypeSetName=DatabaseModel?.TypeSetName??"UnknownTypeSet" ,
                    DestinationTypeSetName = destinationTypeSetName,
                    TypeMappingItems = items 
                } 
            });
            return string.Empty;
        }


        private IEnumerable<ITypeMappingItem> ToTypeMappingItems(string trimmedContextWithoutHeader)
        {
            var stringBuilder = new StringBuilder();
            TypeMappingItem currentTypeMappingItem = null;
            var isInSourceContext = false;
            var isInDestinationContext = false;
            foreach (var currentChar in trimmedContextWithoutHeader)
            {
                stringBuilder.Append(currentChar);
                var currentString = stringBuilder.ToString();
                if(currentString.EndsWith(sourceTypeStartContext) && currentTypeMappingItem == null && !isInSourceContext)
                {
                    currentTypeMappingItem = new TypeMappingItem();
                    isInSourceContext = true;
                    stringBuilder.Clear();
                }

                if(currentString.EndsWith(sourceTypeEndContext) && currentTypeMappingItem != null && isInSourceContext)
                {
                    currentTypeMappingItem.SourceType = currentString.Substring(0, currentString.Length - sourceTypeEndContext.Length);
                    isInSourceContext = false;
                    stringBuilder.Clear();
                }

                if(currentString .EndsWith(sourceDestinationSeparator)
                    && currentTypeMappingItem != null && !isInSourceContext && !isInDestinationContext )
                {
                    if(!String.IsNullOrWhiteSpace(currentString.Substring(0, currentString.Length - sourceDestinationSeparator.Length)))
                    {
                        throw new Exception("Formatting errors");
                    }
                    stringBuilder.Clear();
                }

                if (currentString.EndsWith(destinationTypeStartContext)
                    && currentTypeMappingItem != null && !isInSourceContext && !isInDestinationContext)
                {
                    if (!String.IsNullOrWhiteSpace(currentString.Substring(0, currentString.Length - destinationTypeStartContext.Length)))
                    {
                        throw new Exception("Formatting errors");
                    }
                    isInDestinationContext = true;
                    stringBuilder.Clear();
                }

                if (currentString.EndsWith(destinationTypeEndContext)
                    && currentTypeMappingItem != null && !isInSourceContext && isInDestinationContext)
                {
                    currentTypeMappingItem.DestinationType = currentString.Substring(0, currentString.Length - destinationTypeEndContext.Length);
                    isInDestinationContext = false;
                    yield return currentTypeMappingItem;
                    currentTypeMappingItem = null;
                    stringBuilder.Clear();
                }
            }
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
            var header = Regex.Split(headerString, "\\)<-\\]\\s*<=>\\s*\\[").First();
            header = header.Substring("[->(".Length);
            return header;
        }

        private TypeMappingItem ToTypeMappingItem(string itemAsString)
        {
            var splitted = Regex.Split(itemAsString, "\\)<-\\]\\s*=>\\s*\\[->\\(").ToList();
            var sourceType = splitted[0].TrimStart().Substring("[->(".Length);
            var destinationType = splitted[1].TrimEnd();
            if (destinationType.EndsWith(",")) destinationType = destinationType.Substring(0, destinationType.Length - 1);
            destinationType = destinationType.TrimEnd().Substring(0, destinationType.Length - ")<-]".Length);
            var result = new TypeMappingItem() { SourceType = sourceType, DestinationType = destinationType };
            return result;
        }


        public string ToContextString(ITypeMapping converted)
        {
            if (converted == null) return string.Empty;
            if (!converted.TypeMappingItems?.Any()??false) return string.Empty;
            return @$"{StartContext}([->({converted.DestinationTypeSetName})<-]<=>[{ToConstextString(converted.TypeMappingItems)}
]{EndContext}";
        }

        private string ToConstextString(IList<ITypeMappingItem> typeMappingItems)
        {
            return string.Join($",{Environment.NewLine}", typeMappingItems.Select(m => $"[->({m.SourceType})<-]=>[->({m.DestinationType})<-]"));
        }

    }
}
