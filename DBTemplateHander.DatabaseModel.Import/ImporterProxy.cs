using DBTemplateHander.DatabaseModel.Import.Importer;
using DBTemplateHandler.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBTemplateHander.DatabaseModel.Import
{
    public class ImporterProxy
    {
        private static readonly IList<IImporter> importerRegister = new List<IImporter>()
        {
            new SQLServerImporter(),
        };


        private readonly IDictionary<string, IImporter> ImporterByManagedDbSystem;

        public ImporterProxy()
        {
            ImporterByManagedDbSystem = importerRegister.ToDictionary(m => m.ManagedDbSystem, m => m);
        }

        public IList<string> GetAllManagedDbSystems()
        {
            return ImporterByManagedDbSystem.Keys.ToList();
        }

        public IDatabaseModel ResolveAndImport(string ImportManagedDbName,string connectionString)
        {
            if (!ImporterByManagedDbSystem.TryGetValue(ImportManagedDbName, out var importer)) 
                throw new ArgumentException("not in managed register", nameof(ImportManagedDbName));
            return importer.Import(connectionString);
        }
    }
}
