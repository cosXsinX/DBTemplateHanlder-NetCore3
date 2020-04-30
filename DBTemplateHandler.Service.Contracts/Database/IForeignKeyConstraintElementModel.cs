namespace DBTemplateHandler.Core.Database
{
    public interface IForeignKeyConstraintElementModel
    {
        public IColumnReferenceModel Primary { get; set; }
        public IColumnReferenceModel Foreign { get; set; }
    }
}
