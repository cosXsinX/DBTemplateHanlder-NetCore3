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

        public Task<IList<ITemplateModel>> GetTemplateModels()
        {
            return Task.FromResult(persistenceFacade.GetAllTemplateModel());
        }

        public Task<IList<IDatabaseModel>> GetDatabaseModels()
        {
            return Task.FromResult(persistenceFacade.GetAllDatabaseModel());
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
