using System.Collections.Generic;

namespace DBTemplateHandler.Service.Contracts.TypeMapping
{
    public interface ITypeMapping
    {
        string DestinationTypeSetName { get; set; }
        string SourceTypeSetName { get; set; }
        IList<ITypeMappingItem> TypeMappingItems { get; set; }
    }
}