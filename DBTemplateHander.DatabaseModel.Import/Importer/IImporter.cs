using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHander.DatabaseModel.Import.Importer
{
    public interface IImporter
    {
        public string ManagedDbSystem { get; }
        public IDatabaseModel Import(string connectionString);
    }
}
