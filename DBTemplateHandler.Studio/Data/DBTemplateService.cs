using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Persistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DBTemplateHander.DatabaseModel.Import;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Persistance.Serializable;
using DBTemplateHandler.Service.Contracts.TypeMapping;

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

        public Task<IList<ITemplateContextHandlerIdentity>> GetAllItemplateContextHandlerIdentity()
        {
            return Task.FromResult(inputModelHandler.GetAllItemplateContextHandlerIdentity());
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

        private IList<string> ToAllDataTypesInternal(IDatabaseModel databaseModel)
        {
            if (databaseModel == null) new List<string>();
            var tableModels = databaseModel?.Tables ?? new List<ITableModel>();
            var columnModels = tableModels.SelectMany(m => m?.Columns ?? new List<IColumnModel>());
            return columnModels.Select(m => m.Type).Distinct().ToList();
        }

        public Task<IList<string>> ToAllDataTypes(IDatabaseModel databaseModel)
        {
            return Task.FromResult(ToAllDataTypesInternal(databaseModel));
        }

        public IDatabaseModel GetDatabaseModelByPersistenceNameSync(string persistenceName)
        {
            return persistenceFacade.GetDatabaseModelByPersistenceName(persistenceName);
        }

        public Task<IList<IDatabaseModel>> GetDatabaseModels()
        {
            return Task.FromResult(persistenceFacade.GetAllDatabaseModels());
        }


        public Task<IList<string>> GetAllTypeSetsPersistenceNames()
        {
            return Task.FromResult(persistenceFacade.GetAllTypeSetPersistenceNames());
        }
        public Task<IList<TypeSetItem>> GetTypeSetByPersistenceName(string persistenceName)
        {
            return Task.FromResult(persistenceFacade.GetTypeSetByPersistenceName(persistenceName));
        }

        public void SaveTypeSet(string typeSetName, IList<TypeSetItem> typeSet)
        {
            persistenceFacade.SaveTypeSet(typeSetName, typeSet);
        }

        public void DeleteTypeSet(string typeSetName)
        {
            persistenceFacade.DeleteTypeSet(typeSetName);
        }


        public Task<IList<string>> GetAllTypeMappingPersistenceNames()
        {
            return Task.FromResult(persistenceFacade.GetAllTypeMappingPersistenceNames());
        }

        public Task<DBTemplateHandler.Persistance.Serializable.TypeMapping> GetTypeMappingByPersistenceName(string persistenceName)
        {
            return Task.FromResult(persistenceFacade.GetTypeMappingByPersistenceName(persistenceName));
        }

        public void SaveTypeMapping(string typeMappingName, DBTemplateHandler.Persistance.Serializable.TypeMapping typeMapping)
        {
            persistenceFacade.SaveTypeMapping(typeMappingName, typeMapping);
        }

        public void DeleteTypeMapping(string typeMappingName)
        {
            persistenceFacade.DeleteTypeMapping(typeMappingName);
        }

        public IList<IHandledTemplateResultModel> Process(ITemplateModel templateModel,IDatabaseModel databaseModel)
        {
            var typeMappings = persistenceFacade.GetAllTypeMapping();
            var sourceTypeAssociatedTypeMappings = typeMappings.Where(m => m.SourceTypeSetName == databaseModel.TypeSetName).Select(ToTypeMapping).ToList();

            IDatabaseTemplateHandlerInputModel databaseTemplateHandlerInputModel = new DatabaseTemplateHandlerInputModel()
            {
                DatabaseModel = databaseModel,
                TemplateModels = new List<ITemplateModel>()
                {
                    templateModel
                },
                typeMappings = sourceTypeAssociatedTypeMappings
            };
            var result = inputModelHandler.Process(databaseTemplateHandlerInputModel);
            return result;
        }

        private ITypeMapping ToTypeMapping(DBTemplateHandler.Persistance.Serializable.TypeMapping converted)
        {
            var result = new TypeMapping();
            result.DestinationTypeSetName = converted.DestinationTypeSetName;
            result.SourceTypeSetName = converted.SourceTypeSetName;
            result.TypeMappingItems = (converted?.TypeMappingItems ?? new List<DBTemplateHandler.Persistance.Serializable.TypeMappingItem>()).Select(ToTypeMappingItem).ToList();
            return result;
        }

        public ITypeMappingItem ToTypeMappingItem(DBTemplateHandler.Persistance.Serializable.TypeMappingItem converted)
        {
            var result = new TypeMappingItem();
            result.DestinationType = converted.DestinationType;
            result.SourceType = converted.SourceType;
            return result;
        }

        private class TypeMapping : ITypeMapping
        {
            public string DestinationTypeSetName { get; set ; }
            public string SourceTypeSetName { get ; set ; }
            public IList<ITypeMappingItem> TypeMappingItems { get ; set ; }
        }

        private class TypeMappingItem : ITypeMappingItem
        {
            public string DestinationType { get; set; }
            public string SourceType { get ; set ; }
        }
    }
}
