using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TypeMapping
{
    public class TypeMappingConverter
    {
        private TypeMappingItemConverter typeMappingItemConverter = new TypeMappingItemConverter();

        public TypeMapping ToTypeMapping(ITypeMapping converted)
        {
            var result = new TypeMapping();
            result.DestinationTypeSetName = converted.DestinationTypeSetName;
            result.SourceTypeSetName = converted.SourceTypeSetName;
            result.TypeMappingItems = (converted.TypeMappingItems ?? new List<ITypeMappingItem>()).Select(typeMappingItemConverter.ToTypeMappingItem)
                .Cast<ITypeMappingItem>().ToList();
            return result;
        }
    }
}
