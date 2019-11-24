using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Database.MetaDescriptors;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Persistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DBTemplateHander.DatabaseModel.Import;

namespace DBTemplateHandler.Studio.Data
{
    public class DBTemplateService
    {
        private readonly InputModelHandler inputModelHandler = new InputModelHandler();
        private readonly PersistenceFacade persistenceFacade;
        public DBTemplateService(PersistenceFacade persistenceFacade)
        {
            if (persistenceFacade == null) throw new ArgumentNullException(nameof(persistenceFacade));
            this.persistenceFacade = persistenceFacade;
        }

        public class Config
        {
            public PersistenceFacadeConfiguration PersistenceConfig { get; set; }
        }

        public string ColumnTemplateFileNameWord
        {
            get => inputModelHandler?.ColumnTemplateFileNameWord ?? string.Empty;
        }

        public string TableFilePathTemplateWord
        {
            get => inputModelHandler?.TableFilePathTemplateWord ?? string.Empty;
        }

        public string DatabaseFilePathTemplateWord
        {
            get => inputModelHandler?.DatabaseFilePathTemplateWord ?? string.Empty;
        }

        public IList<string> AllFilePathTemplateWords
        {
            get => inputModelHandler?.AllFilePathTemplateWords??new List<string>();
        }

        public void SaveDatabaseModel(string databaseModelPersistenceName, IDatabaseModel databaseModel)
        {
            if (String.IsNullOrWhiteSpace(databaseModelPersistenceName)) return;
            persistenceFacade.Save(databaseModelPersistenceName, databaseModel);
        }

        public void DeleteDatabaseModel(string databaseModelPersistenceName)
        {
            persistenceFacade.DeleteDatabaseModel(databaseModelPersistenceName);
        }

        public void SaveTemplateModel(string templateGroupName, ITemplateModel templateModel)
        {
            var templateModels = persistenceFacade.GetTemplateModelsByTemplateGroupName(templateGroupName);
            var notModifiedTemplateModels = templateModels.Where(m => m.TemplatedFilePath == templateModel.TemplatedFilePath);
            var savedTemplateModels = notModifiedTemplateModels.Concat(Enumerable.Repeat(templateModel,1)).ToList();
            persistenceFacade.Save(templateGroupName,savedTemplateModels);
        }

        public void SaveTemplateModels(string templateGroupName, IList<ITemplateModel> templateModels)
        {
            if (String.IsNullOrWhiteSpace(templateGroupName)) return;
            if (templateModels == null || !templateGroupName.Any()) return;
            persistenceFacade.Save(templateGroupName, templateModels);
        }

        SQLLiteDatabaseDescriptor descriptor = new SQLLiteDatabaseDescriptor();
        public Task<IList<string>> GetAllAvailableColumnTypes()
        {
            IList<string> result = descriptor.GetPossibleColumnTypes().ToList();
            return Task.FromResult(result);
        }

        public void DeleteTemplateModels(string templateGroupName)
        {
            persistenceFacade.DeleteTemplates(templateGroupName);
        }

        public Task<IList<ITemplateModel>> GetTemplateModels()
        {
            return Task.FromResult(persistenceFacade.GetAllTemplateModel());
        }

        public Task<IList<string>> GetAllTemplateModelPersistenceNames()
        {
            return Task.FromResult(persistenceFacade.GetAllTemplatesPersistanceNames());
        }

        public Task<IList<ITemplateModel>> GetTemplateModelByPersistenceName(string persistenceName)
        {
            return Task.FromResult(persistenceFacade.GetTemplateModelsByTemplateGroupName(persistenceName));
        }

        public IList<ITemplateModel> GetTemplateModelByPersistenceNameSync(string persistenceName)
        {
            return persistenceFacade.GetTemplateModelsByTemplateGroupName(persistenceName);
        }

        public Task<IList<string>> GetDatabaseModelPeristenceNames()
        {
            return Task.FromResult(persistenceFacade.GetAllDatabaseModelPersistenceNames());
        }

        public Task<IDatabaseModel> GetDatabaseModelByPersistenceName(string persistenceName)
        {
            return Task.FromResult(persistenceFacade.GetDatabaseModelByPersistenceName(persistenceName));
        }

        public IDatabaseModel GetDatabaseModelByPersistenceNameSync(string persistenceName)
        {
            return persistenceFacade.GetDatabaseModelByPersistenceName(persistenceName);
        }

        public Task<IList<IDatabaseModel>> GetDatabaseModels()
        {
            return Task.FromResult(persistenceFacade.GetAllDatabaseModels());
        }

        public IList<IHandledTemplateResultModel> Process(ITemplateModel templateModel,IDatabaseModel databaseModel)
        {
            IDatabaseTemplateHandlerInputModel databaseTemplateHandlerInputModel = new DatabaseTemplateHandlerInputModel()
            {
                DatabaseModel = databaseModel,
                TemplateModels = new List<ITemplateModel>()
                {
                    templateModel
                }
            };
            var result = inputModelHandler.Process(databaseTemplateHandlerInputModel);
            return result;
        }
    }
}
