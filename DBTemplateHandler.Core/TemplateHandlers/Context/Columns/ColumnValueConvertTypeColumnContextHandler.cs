using DBTemplateHandler.Core.Database;
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
            if (typeMappings == null) return;
            if (!typeMappings.Any()) return;
            conversionMap = ToConversionMap(typeMappings);
        }

        private IDictionary<MappingKey,string> ToConversionMap(IList<ITypeMapping> typeMappings)
        {
            var intermediateResult = typeMappings.SelectMany(m => (m?.TypeMappingItems ?? new List<ITypeMappingItem>())
                .Select(i => Tuple.Create(new MappingKey() { DestinationTypeSet = m.DestinationTypeSetName, SourceType = i.SourceType }, i.DestinationType))).ToList();
            var result = intermediateResult.GroupBy(m => m.Item1, m => m.Item2).ToDictionary(m => m.Key, m => m.First());
            return result;
        }

        private struct MappingKey
        {
            public string SourceType { get; set; }
            public string DestinationTypeSet { get; set; }

            public bool Equals(object other)
            {
                if (!(other is MappingKey)) return false;
                var otherAsMappingKey = (MappingKey)other;
                if (DestinationTypeSet.Equals(otherAsMappingKey.DestinationTypeSet)) return false;
                if (SourceType != otherAsMappingKey.SourceType) return false;
                return true;
            }
        }

        private readonly IDictionary<MappingKey, string> conversionMap = new Dictionary<MappingKey, string>();

        private HashSet<string> DestinationTypeSets;
        private bool InitConversionHandlerMap()
        {

            if (DestinationTypeSets != null) return true;
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
            if (!DestinationTypeSets.Contains(TrimedStringContext)) return $"CONVERT:UNKNOWN({TrimedStringContext})";
            if (conversionMap.TryGetValue(new MappingKey() { DestinationTypeSet = TrimedStringContext, SourceType = ColumnModel.Type }, out var result)) return result;
            return ColumnModel.Type;
        }



        public override bool isStartContextAndEndContextAnEntireWord => false;

    }
}
