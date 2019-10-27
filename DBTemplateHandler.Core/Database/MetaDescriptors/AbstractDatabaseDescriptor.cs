using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.Database.MetaDescriptors
{
    public abstract class AbstractDatabaseDescriptor
    {
        public abstract string[] get_possibleColumnTypes();

        public abstract string ConvertType(string ConvertedType, string DestinationEnvironment);

        public abstract string ConvertTypeToJava(string ConvertedType);
    }
}
