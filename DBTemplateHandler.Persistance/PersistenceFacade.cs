using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DBTemplateHandler.Persistance
{
    public class PersistenceFacade
    {
        private readonly Persistor<IList<TemplateModel>> templateModelPersistor;
        private readonly Persistor<DatabaseModel> databaseModelPersistor;
        private readonly string templatesFolderPath;
        private readonly string databaseModelsFolderPath;
        public PersistenceFacade()
        {
            var applicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            templatesFolderPath = Path.Combine(applicationDataFolder, "templates");
            if (!Directory.Exists(templatesFolderPath)) Directory.CreateDirectory(templatesFolderPath);
            templateModelPersistor = new Persistor<IList<TemplateModel>>(templatesFolderPath);

            databaseModelsFolderPath = Path.Combine(applicationDataFolder, "databaseModels");
            if (!Directory.Exists(databaseModelsFolderPath)) Directory.CreateDirectory(databaseModelsFolderPath);
            databaseModelPersistor = new Persistor<DatabaseModel>(databaseModelsFolderPath);
        }

        public IList<ITemplateModel> GetAllTemplateModel()
        {
            return templateModelPersistor.GetAll().SelectMany(m => m).Cast<ITemplateModel>().ToList();
        }

        private TemplateModel ToTemplateModel(ITemplateModel converted)
        {
            var result = new TemplateModel();
            result.TemplatedFileContent = converted.TemplatedFileContent;
            result.TemplatedFilePath = converted.TemplatedFilePath;
            return result;
        }

        public void Save(string templatesPersistenceName, IList<ITemplateModel> templateGroup)
        {
            templateModelPersistor.Save(templatesPersistenceName, templateGroup.Select(ToTemplateModel).ToList());
        }

        public IList<string> GetAllTemplatesPersistanceNames()
        {
            return templateModelPersistor.GetAllPersistanceNames();
        }

        public IList<ITemplateModel> GetTemplateModelsByTemplateGroupName(string templateGroupName)
        {
            return templateModelPersistor.GetByPersistenceName(templateGroupName).Cast<ITemplateModel>().ToList();
        }

        public IDatabaseModel GetDatabaseModelByPersistenceName(string persistenceName)
        {
            return databaseModelPersistor.GetByPersistenceName(persistenceName);
        }

        public IList<string> GetAllDatabaseModelPersistenceNames()
        {
            return databaseModelPersistor.GetAllPersistanceNames();
        }

        public IList<IDatabaseModel> GetAllDatabaseModels()
        {
            return databaseModelPersistor.GetAll().Cast<IDatabaseModel>().ToList();
        }

        private DatabaseModel ToDatabaseModel(IDatabaseModel databaseModel)
        {
            var result = new DatabaseModel();
            result.Name = databaseModel.Name;
            result.Tables = databaseModel.Tables;
            return result;
        }

        public void Save(string databaseModelPersistenceName , IDatabaseModel databaseModel)
        {
            databaseModelPersistor.Save(databaseModelPersistenceName, ToDatabaseModel(databaseModel));
        }
    }
}
