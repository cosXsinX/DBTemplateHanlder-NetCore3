using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace DBTemplateHandler.Persistance
{
    public class Persistor<T>
    {
        private readonly string _persistenceFolder;

        public Persistor(string persistenceFolder)
        {
            if (string.IsNullOrEmpty(persistenceFolder)) throw new ArgumentException("Cannot be null or empty", nameof(persistenceFolder));
            _persistenceFolder = persistenceFolder;
        }

        public IList<string> GetAllPersistanceNames()
        {
            if (!Directory.Exists(_persistenceFolder)) return new List<string>();
            var files = Directory.GetFiles(_persistenceFolder, "*.json", new EnumerationOptions() { RecurseSubdirectories = false });
            var templateGroupNames = files.Select(fileName => fileName.Substring(0, fileName.Length - ".json".Length)).ToList();
            return templateGroupNames;
        }

        public IList<T> GetAll()
        {
            var allTemplateGroupNames = GetAllPersistanceNames();
            var templateModels = allTemplateGroupNames.Select(m => GetByPersistenceName(m)).ToList();
            return templateModels;
        }

        public T GetByPersistenceName(string templateGroupName)
        {
            var templateGroupFileName = string.Concat(templateGroupName, ".json");
            var filePath = Path.Combine(_persistenceFolder, templateGroupFileName);
            if (!File.Exists(filePath)) return default(T);
            var fileContent = File.ReadAllText(filePath);
            var templateModels = JsonSerializer.Deserialize<T>(fileContent);
            return templateModels;
        }

        public void Save(string templatesPersistenceName, T persisted)
        {
            var serializedTemplateGroup = JsonSerializer.Serialize(persisted);
            var saveFilePath = Path.Combine(_persistenceFolder, string.Concat(templatesPersistenceName, ".json"));
            if (File.Exists(saveFilePath)) File.Delete(saveFilePath);
            File.WriteAllText(saveFilePath, serializedTemplateGroup);
        }
    }
}
