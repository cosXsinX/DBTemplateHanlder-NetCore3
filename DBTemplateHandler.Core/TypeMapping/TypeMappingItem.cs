using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TypeMapping
{
    public class TypeMappingItem : ITypeMappingItem
    {
        public string DestinationType { get; set; }
        public string SourceType { get; set; }
    }
}
