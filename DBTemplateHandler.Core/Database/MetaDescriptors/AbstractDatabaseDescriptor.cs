using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database.MetaDescriptors
{
    public abstract class AbstractDatabaseDescriptor
    {
        public abstract String[] get_possibleColumnTypes();

        public abstract String ConvertType(String ConvertedType, String DestinationEnvironment);

        public abstract String ConvertTypeToJava(String ConvertedType);
    }
}
