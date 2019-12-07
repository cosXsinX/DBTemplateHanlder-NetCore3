using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TypeMapping
{
    public class TypeMappingItemConverter
    {
        public TypeMappingItem ToTypeMappingItem(ITypeMappingItem converted)
        {
            var result = new TypeMappingItem();
            result.DestinationType = converted.DestinationType;
            result.SourceType = converted.SourceType;
            return result;
        }
    }
}
