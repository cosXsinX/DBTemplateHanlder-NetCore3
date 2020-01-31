﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnValueConvertTypeColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(";
        public override string EndContext => ")::}";
        public override string ContextActionDescription => "Is replaced by the specified language current column value type conversion (ex: Java, CSharp, ...)";

        public ColumnValueConvertTypeColumnContextHandler(TemplateHandlerNew templateHandlerNew,IList<ITypeMapping> typeMappings):base(templateHandlerNew)
        {

        }

        private IDictionary<MappingKey,string> ToConversionMap(IList<ITypeMapping> typeMappings)
        {
            if (typeMappings == null || !typeMappings.Any()) return new Dictionary<MappingKey, string>();
            var intermediateResult = typeMappings.SelectMany(m => (m?.TypeMappingItems ?? new List<ITypeMappingItem>())
                .Select(i => Tuple.Create(new MappingKey() { DestinationTypeSet = (m.DestinationTypeSetName??string.Empty).ToLowerInvariant(), SourceType = i.SourceType }, i.DestinationType))).ToList();
            var result = intermediateResult.GroupBy(m => m.Item1, m => m.Item2).ToDictionary(m => m.Key, m => m.First());
            return result;
        }

        private struct MappingKey
        {
            public string SourceType { get; set; }
            public string DestinationTypeSet { get; set; }

            public override bool Equals(object obj)
            {
                return obj is MappingKey key &&
                       SourceType == key.SourceType &&
                       DestinationTypeSet == key.DestinationTypeSet;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(SourceType, DestinationTypeSet);
            }
        }

        private HashSet<string> DestinationTypeSets;
        private IDictionary<MappingKey, string> conversionMap;
        private bool InitConversionHandlerMap()
        {
            if (DestinationTypeSets != null) return true;
            conversionMap = ToConversionMap(TemplateHandlerNew.TypeMappings);
            DestinationTypeSets = new HashSet<string>(conversionMap.Keys.Select(m => m.DestinationTypeSet));
            return (DestinationTypeSets != null);
        }

        public override string processContext(string StringContext)
        {
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel descriptionPojo = ColumnModel;
            if (descriptionPojo == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (TrimedStringContext == "")
                throw new Exception("There is a problem with the function provided in template '" +
                        (StartContext + TrimedStringContext + EndContext) +
                            "' -> The value parameter cannot be empty");
            InitConversionHandlerMap();
            if (!DestinationTypeSets.Contains(TrimedStringContext.ToLowerInvariant())) return $"CONVERT:UNKNOWN({TrimedStringContext})";
            if (conversionMap.TryGetValue(new MappingKey() { DestinationTypeSet = TrimedStringContext.ToLowerInvariant(), SourceType = ColumnModel.Type }, out var result))
            {
                var processedResult = TemplateHandlerNew.HandleTableColumnTemplate(result, ColumnModel);
                return processedResult;
            }
             
            return ColumnModel.Type;
        }

        public override bool isStartContextAndEndContextAnEntireWord => false;

    }
}
