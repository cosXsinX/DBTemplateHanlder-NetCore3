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


        private const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(";
        private const String END_CONTEXT_WORD = ")::}";

        public const String TEMPLATE_TABLE_WORD = START_CONTEXT_WORD + END_CONTEXT_WORD;


        public override String getStartContextStringWrapper()
        {
            return START_CONTEXT_WORD;
        }


        public override String getEndContextStringWrapper()
        {
            return END_CONTEXT_WORD;
        }

        IDictionary<String, AbstractConversionHandler> ConversionHandlerMap = null;
        private bool InitConversionHandlerMap()
        {
            if (ConversionHandlerMap != null) return true;
            ConversionHandlerMap = new Dictionary<String, ColumnValueConvertTypeColumnContextHandler.AbstractConversionHandler>();
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


        public override String processContext(String StringContext)
        {
            if (StringContext == null)
                throw new Exception("The provided StringContext is null");
            TableColumnDescriptionPOJO descriptionPojo = getAssociatedColumnDescriptorPOJO();
            if (descriptionPojo == null)
                throw new Exception("The AssociatedColumnDescriptorPOJO is not set");

            String TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (TrimedStringContext == "")
                throw new Exception("There is a problem with the function provided in template '" +
                        (START_CONTEXT_WORD + TrimedStringContext + END_CONTEXT_WORD) +
                            "' -> The value parameter cannot be empty");
            AbstractConversionHandler abstractConversionHandler =
                    GetConversionHandlerForEnvironmentDestinationKey
                        (TrimedStringContext);
            return abstractConversionHandler.ConvertType(descriptionPojo.get_TypeStr());//TODO Here provide database descriptor throughout pojo object
        }

        private abstract class AbstractConversionHandler
        {
            public abstract AbstractDatabaseDescriptor getDatabaseDescriptor();

            public abstract void setDatabaseDescriptor(AbstractDatabaseDescriptor descriptor);

            public abstract String getTargetEnvironmentKey();

            public abstract String ConvertType(String ConvertedTypeString);
        }

        private class JavaConversionHandler : AbstractConversionHandler
        {
            private const String TARGET_CONVERSION_KEY = "JAVA";
            public override String getTargetEnvironmentKey()
            {
                return TARGET_CONVERSION_KEY.ToUpper();
            }


            public override String ConvertType(String ConvertedTypeString)
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

        public override bool isStartContextAndEndContextAnEntireWord()
        {
            return false;
        }
    }
}
