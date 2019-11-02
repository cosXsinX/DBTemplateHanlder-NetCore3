namespace DBTemplateHandler.Core.Template
{
    public interface IHandledTemplateResultModel
    {
        string Content { get; set; }
        string Path { get; set; }
    }
}