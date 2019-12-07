using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Persistance.Conversion;
using DBTemplateHandler.Persistance.Serializable;
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
        private readonly Persistor<PersistableDatabaseModel> databaseModelPersistor;
        private readonly Persistor<TypeMapping> typeMappingPersistor;
        private readonly Persistor<IList<TypeSetItem>> typeSetPersistor;
        private readonly DatabaseModelConverter databaseModelConverter = new DatabaseModelConverter();
        private readonly string templatesFolderPath;
        private readonly string databaseModelsFolderPath;
        private readonly string typeMappingFolderPath;
        private readonly string typeSetFolderPath;
        public PersistenceFacade(PersistenceFacadeConfiguration persistenceFacadeConfiguration)
        {
            if (persistenceFacadeConfiguration == null) throw new ArgumentNullException(nameof(persistenceFacadeConfiguration));
            
            templatesFolderPath = persistenceFacadeConfiguration.TemplatesFolderPath; 
            if (!Directory.Exists(templatesFolderPath)) Directory.CreateDirectory(templatesFolderPath);
            templateModelPersistor = new Persistor<IList<TemplateModel>>(templatesFolderPath);

            databaseModelsFolderPath = persistenceFacadeConfiguration.DatabaseModelsFolderPath;
            if (!Directory.Exists(databaseModelsFolderPath)) Directory.CreateDirectory(databaseModelsFolderPath);
            databaseModelPersistor = new Persistor<PersistableDatabaseModel>(databaseModelsFolderPath);

            typeMappingFolderPath = persistenceFacadeConfiguration.TypeMappingFolderPath;
            if (!Directory.Exists(typeMappingFolderPath)) Directory.CreateDirectory(typeMappingFolderPath);
            typeMappingPersistor = new Persistor<TypeMapping>(typeMappingFolderPath);

            typeSetFolderPath = persistenceFacadeConfiguration.TypeSetFolderPath;
            if (!Directory.Exists(typeSetFolderPath)) Directory.CreateDirectory(typeSetFolderPath);
            typeSetPersistor = new Persistor<IList<TypeSetItem>>(typeSetFolderPath);
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

        public void DeleteTemplates(string templatesPersistenceName)
        {
            templateModelPersistor.Delete(templatesPersistenceName);
        }

        public IList<string> GetAllTemplatesPersistanceNames()
        {
            return templateModelPersistor.GetAllPersistanceNames();
        }

        public IList<ITemplateModel> GetTemplateModelsByTemplateGroupName(string templateGroupName)
        {
            var templateGroup = templateModelPersistor.GetByPersistenceName(templateGroupName);
            if (templateGroup == null) return null;
            return templateGroup.Cast<ITemplateModel>().ToList();
        }

        public IDatabaseModel GetDatabaseModelByPersistenceName(string persistenceName)
        {
            var intermediateResuts = databaseModelPersistor.GetByPersistenceName(persistenceName);
            var results = databaseModelConverter.ToUnPersisted(intermediateResuts);
            return results;
        }

        public IList<string> GetAllDatabaseModelPersistenceNames()
        {
            return databaseModelPersistor.GetAllPersistanceNames();
        }

        public IList<IDatabaseModel> GetAllDatabaseModels()
        {
            return databaseModelPersistor.GetAll().Cast<IDatabaseModel>().ToList();
        }

        public void Save(string databaseModelPersistenceName , IDatabaseModel databaseModel)
        {
            var persistable = databaseModelConverter.ToPersistable(databaseModel);
            databaseModelPersistor.Save(databaseModelPersistenceName, persistable);
        }

        public void DeleteDatabaseModel(string databaseModelPersistenceName)
        {
            databaseModelPersistor.Delete(databaseModelPersistenceName);
        }

        public IList<string> GetAllTypeMappingPersistenceNames()
        {
            return typeMappingPersistor.GetAllPersistanceNames();
        }

        public TypeMapping GetTypeMappingByPersistenceName(string persistenceName)
        {
            var result = typeMappingPersistor.GetByPersistenceName(persistenceName);
            return result;
        }

        public IList<TypeMapping> GetAllTypeMapping()
        {
            return typeMappingPersistor.GetAll().Cast<TypeMapping>().ToList();
        }

        public void SaveTypeMapping(string persistenceName, TypeMapping typeMapping)
        {
            typeMappingPersistor.Save(persistenceName, typeMapping);
        }

        public void DeleteTypeMapping(string persistenceName)
        {
            typeMappingPersistor.Delete(persistenceName);
        }

        public IList<string> GetAllTypeSetPersistenceNames()
        {
            return typeSetPersistor.GetAllPersistanceNames();
        }

        public IList<TypeSetItem> GetTypeSetByPersistenceName(string persistenceName)
        {
            var results = typeSetPersistor.GetByPersistenceName(persistenceName);
            return results;
        }

        public IList<IList<TypeSetItem>> GetAllTypeSets()
        {
            return typeSetPersistor.GetAll().Cast<IList<TypeSetItem>>().ToList();
        }

        public void SaveTypeSet(string persistenceName, IList<TypeSetItem> typeSet)
        {
            typeSetPersistor.Save(persistenceName, typeSet);
        }

        public void DeleteTypeSet(string persistenceName)
        {
            typeSetPersistor.Delete(persistenceName);
        }
    }
}
