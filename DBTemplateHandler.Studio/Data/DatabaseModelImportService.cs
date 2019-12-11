using DBTemplateHander.DatabaseModel.Import;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Persistance;
using DBTemplateHandler.Persistance.Serializable;
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
            var typeSetItems = ToTypeSetItems(databaseModel);
            var alreadyPersistedTypeSetItems = persistenceFacade.GetTypeSetByPersistenceName(databaseModel.TypeSetName);
            var mergedTypeSetItems = Merge(typeSetItems, alreadyPersistedTypeSetItems);
            if(typeSetItems.Any())persistenceFacade.SaveTypeSet(databaseModel.TypeSetName, mergedTypeSetItems);
            return "Import done";
        }

        private IList<TypeSetItem> Merge(IList<TypeSetItem> destination, IList<TypeSetItem> merged)
        {
            var destinationNames = new HashSet<string>(destination.Select(m => m?.Name?? string.Empty).Distinct());
            var result = destination.Concat(merged.Where(m => !destinationNames.Contains(m?.Name ?? string.Empty))).ToList();
            return result;
        }

        private IList<TypeSetItem> ToTypeSetItems(IDatabaseModel databaseModel)
        {
            if (databaseModel == null) new List<string>();
            var tableModels = databaseModel?.Tables ?? new List<ITableModel>();
            var columnModels = tableModels.SelectMany(m => m?.Columns ?? new List<IColumnModel>());
            return columnModels.Select(m => m.Type).Distinct().Where(m => !String.IsNullOrWhiteSpace(m))
                .Select(m => new TypeSetItem() { Name = m }).ToList();
        }

    }
}
