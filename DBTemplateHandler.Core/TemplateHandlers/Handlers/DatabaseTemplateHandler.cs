﻿using System;
using System.Collections.Generic;
using System.IO;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Database.MetaDescriptors;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class DatabaseTemplateHandler
    {
        DatabaseDescriptionPOJO generatedDatabaseDescription;
        AbstractDatabaseDescriptor _databaseDescriptor;

        public DatabaseDescriptionPOJO getGeneratedDatabaseDescription()
        {
            return generatedDatabaseDescription;
        }

        public void setGeneratedDatabaseDescription(DatabaseDescriptionPOJO value)
        {
            generatedDatabaseDescription = value;
        }

        public AbstractDatabaseDescriptor setDatabaseDescriptor()
        {
            return _databaseDescriptor;
        }

        public void setDatabaseDescriptor(AbstractDatabaseDescriptor databaseDescriptor)
        {
            _databaseDescriptor = databaseDescriptor;
        }

        public DatabaseTemplateHandler(DatabaseDescriptionPOJO generatedDatabaseDescription, AbstractDatabaseDescriptor databaseDescriptor)
        {
            setGeneratedDatabaseDescription(generatedDatabaseDescription);
            setDatabaseDescriptor(databaseDescriptor);
        }

        public bool GenerateDatabaseFilesFromTemplateFile(string templateFilePath)
        {
            bool result = false;
		if(templateFilePath == null) return result;
		if(generatedDatabaseDescription == null) return result;
        if (!File.Exists(templateFilePath)) return result;
		String savedFileTempalteName = Path.GetFileName(templateFilePath);
		if(savedFileTempalteName.Contains(TemplateSemanticReferenceClass.TEMPLATE_NAME_DATABASE_NAME_WORD_IDENTIFIER))
			savedFileTempalteName = savedFileTempalteName.
                Replace(TemplateSemanticReferenceClass.TEMPLATE_NAME_DATABASE_NAME_WORD_IDENTIFIER, generatedDatabaseDescription.getDatabaseNameStr());
		if(savedFileTempalteName.Contains(TemplateSemanticReferenceClass.TEMPLATE_NAME_TABLE_NAME_WORD_IDENTIFIER))
		{
			List<TableDescriptionPOJO> tableList = generatedDatabaseDescription.getTableList();
			foreach(TableDescriptionPOJO currentTable in tableList)
			{
				TableTemplateHandler tableTemplateHandler =
                        TableTemplateHandler.TableDescriptionPOJOToTableTemplateHandler(currentTable, _databaseDescriptor);
        string currentResult = tableTemplateHandler.generateTableFileFromTemplateFile(templateFilePath,out List<string> errors);
				if(Path.GetFileName(currentResult).Contains(TemplateSemanticReferenceClass.TEMPLATE_NAME_DATABASE_NAME_WORD_IDENTIFIER)){
					String currentNewSavedFileName = Path.GetFileName(currentResult).
                            Replace(TemplateSemanticReferenceClass.TEMPLATE_NAME_DATABASE_NAME_WORD_IDENTIFIER,
                                    generatedDatabaseDescription.getDatabaseNameStr());
					if(currentNewSavedFileName.EndsWith

                            (TemplateSemanticReferenceClass.TEMPLATE_FILE_NAME_EXTENSION, StringComparison.Ordinal))
                        {
						currentNewSavedFileName = currentNewSavedFileName.Substring(0,currentNewSavedFileName.Length-TemplateSemanticReferenceClass.TEMPLATE_FILE_NAME_EXTENSION.Length);
					}
    FileManager.RenameFile(currentResult, currentNewSavedFileName, true); 
				}
				
			}
		}
		
		return result;
	}
}
}
