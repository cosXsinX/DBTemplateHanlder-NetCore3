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


        private const string START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(";
        private const string END_CONTEXT_WORD = ")::}";

        public const string TEMPLATE_TABLE_WORD = START_CONTEXT_WORD + END_CONTEXT_WORD;

        public override string StartContext
        {
            get => START_CONTEXT_WORD;
        }

        public override string EndContext
        {
            get => END_CONTEXT_WORD;
        }

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
                        (START_CONTEXT_WORD + TrimedStringContext + END_CONTEXT_WORD) +
                            "' -> The value parameter cannot be empty");
            AbstractConversionHandler abstractConversionHandler =
                    GetConversionHandlerForEnvironmentDestinationKey
                        (TrimedStringContext);
            return abstractConversionHandler.ConvertType(descriptionPojo.Type);//TODO Here provide database descriptor throughout pojo object
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
