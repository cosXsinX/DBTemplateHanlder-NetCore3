namespace DBTemplateHandler.Core.Template
{
    public interface ITemplateModel
    {
        string TemplatedFileContent { get; set; }
        string TemplatedFilePath { get; set; }
        ITemplateModel Copy();
    }
}