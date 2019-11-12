using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTemplateHandler.Studio.Data
{
    public class DBTemplateService
    {
        private readonly InputModelHandler inputModelHandler = new InputModelHandler();

        private readonly PersistenceFacade persistenceFacade = new PersistenceFacade();
        public DBTemplateService()
        {
        }

        public void SaveDatabaseModel(string databaseModelPersistenceName, IDatabaseModel databaseModel)
        {
            persistenceFacade.Save(databaseModelPersistenceName, databaseModel);
        }

        public void SaveTemplateModel(string templateGroupName, ITemplateModel templateModel)
        {
            var templateModels = persistenceFacade.GetTemplateModelsByTemplateGroupName(templateGroupName);
            var notModifiedTemplateModels = templateModels.Where(m => m.TemplatedFilePath == templateModel.TemplatedFilePath);
            var savedTemplateModels = notModifiedTemplateModels.Concat(Enumerable.Repeat(templateModel,1)).ToList();
            persistenceFacade.Save(templateGroupName,savedTemplateModels);
        }

        public Task<IList<ITemplateModel>> GetTemplateModels()
        {
            return Task.FromResult(persistenceFacade.GetAllTemplateModel());
        }

        public Task<IList<string>> GetDatabaseModelPeristenceNames()
        {
            return Task.FromResult(persistenceFacade.GetAllDatabaseModelPersistenceNames());
        }

        public Task<IDatabaseModel> GetDatabaseModelByPersistenceName(string persistenceName)
        {
            return Task.FromResult(persistenceFacade.GetDatabaseModelByPersistenceName(persistenceName));
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
