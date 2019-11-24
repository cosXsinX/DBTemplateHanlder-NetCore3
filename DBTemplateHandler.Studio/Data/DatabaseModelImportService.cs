using DBTemplateHander.DatabaseModel.Import;
using DBTemplateHandler.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTemplateHandler.Studio.Data
{
    public class DatabaseModelImportService
    {
        private readonly ImporterProxy importerProxy = new ImporterProxy();
        private readonly PersistenceFacade persistenceFacade;

        public DatabaseModelImportService(ImporterProxy importerProxy, PersistenceFacade persistenceFacade)
        {
            if (importerProxy == null) throw new ArgumentNullException(nameof(importerProxy));
            if (persistenceFacade == null) throw new ArgumentNullException(nameof(persistenceFacade));

            this.importerProxy = importerProxy;
            this.persistenceFacade = persistenceFacade;
        }

        public IList<string> GetAllManagedDbSystems()
        {
            return importerProxy.GetAllManagedDbSystems();
        }

        public string ImportFromDatabase(string dbManagedName,string connectionString)
        {
            var databaseModel = importerProxy.ResolveAndImport(dbManagedName, connectionString);
            if (databaseModel == null) return "The database does not exists or the catalog is empty";
            persistenceFacade.Save(databaseModel.Name, databaseModel);
            return "Import done";
        }

    }
}
