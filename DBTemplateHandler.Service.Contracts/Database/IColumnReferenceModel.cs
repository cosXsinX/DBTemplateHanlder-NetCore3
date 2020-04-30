namespace DBTemplateHandler.Core.Database
{
    public interface IColumnReferenceModel
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
    }
}
