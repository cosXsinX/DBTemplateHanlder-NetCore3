namespace DBTemplateHandler.Core.Database
{
    public class ForeignKeyConstraintElementModel : IForeignKeyConstraintElementModel
    {
        public IColumnReferenceModel Primary { get; set; }
        public IColumnReferenceModel Foreign { get; set; }
    }
}
