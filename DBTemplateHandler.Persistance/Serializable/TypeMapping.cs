using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Persistance.Serializable
{
    public class TypeMapping
    {
        public string SourceTypeSetName { get; set; }
        public string DestinationTypeSetName { get; set; }
        public IList<TypeMappingItem> TypeMappingItems { get; set; }
        
    }
}
