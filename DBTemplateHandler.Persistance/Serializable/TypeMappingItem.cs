﻿using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Persistance.Serializable
{
    public class TypeMappingItem : ITypeMappingItem
    {
        public string SourceType { get; set; }
        public string DestinationType { get; set; }
    }
}
