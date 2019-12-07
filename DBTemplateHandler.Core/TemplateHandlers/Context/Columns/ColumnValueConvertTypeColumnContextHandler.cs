using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Database.MetaDescriptors;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnValueConvertTypeColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(";
        public override string EndContext => ")::}";
        public override string ContextActionDescription => "Is replaced by the specified language current column value type conversion (ex: Java, CSharp, ...)";

        public IDictionary<string, string> conversionMap = new Dictionary<string, string> 
        { 
            {"INT->JAVA","int" } 
        };

        IDictionary<String, AbstractConversionHandler> ConversionHandlerMap = null;
        private bool InitConversionHandlerMap()
        {
            if (ConversionHandlerMap != null) return true;
            ConversionHandlerMap = new Dictionary<string, ColumnValueConvertTypeColumnContextHandler.AbstractConversionHandler>();
            JavaConversionHandler javaConversionHandler = new JavaConversionHandler();
            ConversionHandlerMap.Add(javaConversionHandler.getTargetEnvironmentKey(), javaConversionHandler);
            return (ConversionHandlerMap != null);
        }

        private AbstractConversionHandler GetConversionHandlerForEnvironmentDestinationKey(String EnvironmentDestinationKey)
        {
            if (EnvironmentDestinationKey == null) return null;
            if (!InitConversionHandlerMap()) return null;
            if (!ConversionHandlerMap.ContainsKey(EnvironmentDestinationKey.ToUpper())) return null;
            return ConversionHandlerMap[EnvironmentDestinationKey.ToUpper()];
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
            AbstractConversionHandler abstractConversionHandler =
                    GetConversionHandlerForEnvironmentDestinationKey
                        (TrimedStringContext);
            if (abstractConversionHandler == null) return $"CONVERT:UNKNOWN({TrimedStringContext})";
            return abstractConversionHandler.ConvertType(descriptionPojo.Type);
        }

        private abstract class AbstractConversionHandler
        {
            public abstract AbstractDatabaseDescriptor getDatabaseDescriptor();

            public abstract void setDatabaseDescriptor(AbstractDatabaseDescriptor descriptor);

            public abstract string getTargetEnvironmentKey();

            public abstract string ConvertType(string ConvertedTypeString);
        }

        private class JavaConversionHandler : AbstractConversionHandler
        {
            private const string TARGET_CONVERSION_KEY = "JAVA";
            public override string getTargetEnvironmentKey()
            {
                return TARGET_CONVERSION_KEY.ToUpper();
            }


            public override string ConvertType(string ConvertedTypeString)
            {
                if (ConvertedTypeString == null) return null;
                if (ConvertedTypeString.Equals("")) return "";
                return _databaseDescriptor.ConvertTypeToJava(ConvertedTypeString);
            }

            private AbstractDatabaseDescriptor _databaseDescriptor =
                    new SQLLiteDatabaseDescriptor();// TODO Make an other initialization
            public override AbstractDatabaseDescriptor getDatabaseDescriptor()
            {
                return _databaseDescriptor;
            }

            public override void setDatabaseDescriptor(AbstractDatabaseDescriptor descriptor)
            {
                _databaseDescriptor = descriptor;
            }

        }

        public override bool isStartContextAndEndContextAnEntireWord => false;

    }
}
