using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models
{
    public class SQLServerForeignKeyColumnsModel
    {
        /// <summary>
        /// ID of the FOREIGN KEY constraint.
        /// </summary>
        public int constraint_object_id { get; set; }
        /// <summary>
        /// ID of the column, or set of columns, that comprise the FOREIGN KEY (1..n where n=number of columns).
        /// </summary>
        public int constraint_column_id { get; set; }
        /// <summary>
        /// ID of the parent of the constraint, which is the referencing object.
        /// </summary>
        public int parent_object_id { get; set; }
        /// <summary>
        /// ID of the parent column, which is the referencing column.
        /// </summary>
        public int parent_column_id { get; set; }
        /// <summary>
        /// ID of the referenced object, which has the candidate key.
        /// </summary>
        public int referenced_object_id { get; set; }
        /// <summary>
        /// ID of the referenced column (candidate key column).
        /// </summary>
        public int referenced_column_id { get; set; }
    }
}
