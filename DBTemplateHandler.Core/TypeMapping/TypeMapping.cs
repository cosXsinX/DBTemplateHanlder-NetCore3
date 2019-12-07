using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TypeMapping
{
    public class TypeMapping : ITypeMapping
    {
        public string DestinationTypeSetName { get; set; }
        public string SourceTypeSetName { get; set; }
        public IList<ITypeMappingItem> TypeMappingItems { get; set; }
    }
}
