using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnValueConvertTypeColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(";
        public override string EndContext => ")::}";
        public override string ContextActionDescription => "Is replaced by the specified language current column value type conversion (ex: Java, CSharp, ...)";


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

        //TODO Make that parametric
        private IDictionary<MappingKey, string> conversionMap = new Dictionary<MappingKey, string>
        {
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="INT"},"int" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="BIGINT"},"long" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="BOOLEAN"},"boolean" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="CHAR"},"char" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="DATE"},"Date" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="DATETIME"},"Date" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="DECIMAL"},"double" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="INTEGER"},"int" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="NUMERIC"},"double" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="REAL"},"double" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="STRING"},"String" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="TEXT"},"String" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="TIME"},"Date" } ,
            {new MappingKey(){ DestinationTypeSet="JAVA",SourceType="VARCHAR"},"String" } ,

        };

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
