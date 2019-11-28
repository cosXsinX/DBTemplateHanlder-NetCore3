using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models
{
    public class SQLServerIndexColumnsModel
    {
        public int object_id { get; set; }
        public int index_id { get; set; }
        public int index_column_id { get; set; }
        public int column_id { get; set; }
        public short key_ordinal { get; set; }
        public short partition_ordinal { get; set; }
        public bool is_descending_key { get; set; }
        public bool is_included_column { get; set; }
    }
}
