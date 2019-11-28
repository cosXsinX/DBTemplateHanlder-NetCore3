using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer.SQLServerImporterComponents.Models
{
    public class SQLServerIndexesModel
    {
        public int object_id { get; set; }
        public string name { get; set; }
        public int index_id { get; set; }
        public short type { get; set; }
        public string type_desc { get; set; }
        public bool? is_unique { get; set; }
        public int? data_space_id { get; set; }
        public bool? ignore_dup_key { get; set; }
        public bool? is_primary_key { get; set; }
        public bool? is_unique_constraint { get; set; }
        public short fill_factor { get; set; }
        public bool? is_padded { get; set; }
        public bool? is_disabled { get; set; }
        public bool? is_hypothetical { get; set; }
        public bool? is_ignored_in_optimization { get; set; }
        public bool? allow_row_locks { get; set; }
        public bool? allow_page_locks { get; set; }
        public bool? has_filter { get; set; }
        public string filter_definition { get; set; }
        public int? compression_delay { get; set; }
        public bool? suppress_dup_key_messages { get; set; }
        public bool? auto_created { get; set; }
    }
}
