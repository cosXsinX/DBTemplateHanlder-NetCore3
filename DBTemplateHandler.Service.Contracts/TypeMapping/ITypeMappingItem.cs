namespace DBTemplateHandler.Service.Contracts.TypeMapping
{
    public interface ITypeMappingItem
    {
        string DestinationType { get; set; }
        string SourceType { get; set; }
    }
}