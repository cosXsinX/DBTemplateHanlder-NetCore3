using DBTemplateHandler.Core.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DBTemplateHandler.Studio.Data
{
    public class ExportService
    {
        public class Config
        {
            public string ReadyDirectory { get; set; }
            public string WorkingDirectory { get; set; }
        }


        private readonly string WorkingDirectory;
        private readonly string ReadyDirectory;


        public ExportService (Config config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (config.WorkingDirectory == null) throw new ArgumentNullException(string.Concat(".", nameof(config), nameof(config.WorkingDirectory)));
            if (config.ReadyDirectory == null) throw new ArgumentNullException(string.Concat(".", nameof(config), nameof(config.ReadyDirectory)));
            if(!Directory.Exists(config.WorkingDirectory))Directory.CreateDirectory(config.WorkingDirectory);
            if (!Directory.Exists(config.ReadyDirectory)) Directory.CreateDirectory(config.ReadyDirectory);

            WorkingDirectory = config.WorkingDirectory;
            ReadyDirectory = config.ReadyDirectory;
        }

        private readonly object lockObject = new object();
        public string ToZipArchive(string zipArchiveName,IList<IHandledTemplateResultModel> archiveds)
        {
            var zipArchiveNameWorkingDirectory = Path.Combine(WorkingDirectory, zipArchiveName);
            
            lock (lockObject) //TODO better management of concurency to be done => example identifying directory by thread index
            {
                if (Directory.Exists(zipArchiveNameWorkingDirectory))
                {
                    Directory.Delete(zipArchiveNameWorkingDirectory, true);
                }
                Directory.CreateDirectory(zipArchiveNameWorkingDirectory);

                var archivedWithTempFilePath = 
                    archiveds.Select(m => 
                        Tuple.Create(m, Path.Combine(zipArchiveNameWorkingDirectory, m.Path))).ToList();

                var tempFolderToCreate = archivedWithTempFilePath
                    .Select(m => Directory.GetParent(m.Item2)).Distinct().Where(m => !m.Exists).ToList();

                tempFolderToCreate.ForEach(m => Directory.CreateDirectory(m.FullName));

                archivedWithTempFilePath.ForEach(m => File.WriteAllText(m.Item2, m.Item1.Content));

                string archiveFilePathWorking = Path.Combine(Directory.GetParent(zipArchiveNameWorkingDirectory).FullName, $"{zipArchiveName}.zip");
                if (File.Exists(archiveFilePathWorking)) File.Delete(archiveFilePathWorking);
                ZipFile.CreateFromDirectory(zipArchiveNameWorkingDirectory, archiveFilePathWorking);
                Directory.Delete(zipArchiveNameWorkingDirectory, true);
                return archiveFilePathWorking;
            }
        }
    }
}
