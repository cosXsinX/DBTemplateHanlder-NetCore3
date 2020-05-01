using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Persistance;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBTemplateHandler.Core.AcceptanceTests.CSharp
{
    [TestFixture]
    public class CSharpAcceptanceTests
    {
        PersistenceFacade persistanceFacade;
        InputModelHandler _tested = new InputModelHandler();

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public string GetExpectedResultContent(string expectedResultFileName)
        {
            var filePath = Path.Combine(AssemblyDirectory, "CSharp", "ExpectedResults", expectedResultFileName);
            var fileContent = File.ReadAllText(filePath);
            return fileContent;
        }

        public string GetExpectedTemplateContent(string templateFileName)
        {
            var filePath = Path.Combine(AssemblyDirectory, "CSharp", "Templates", templateFileName);
            var fileContent = File.ReadAllText(filePath);
            return fileContent;
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            PersistenceFacadeConfiguration persistanceFacadeConfiguration =
                new PersistenceFacadeConfiguration()
                {
                    DatabaseModelsFolderPath = Path.Combine(AssemblyDirectory, "CSharp","DatabaseModels"),
                    TemplatesFolderPath = Path.Combine(AssemblyDirectory, "CSharp", "Templates"),
                    TypeMappingFolderPath = Path.Combine(AssemblyDirectory, "CSharp", "TypeMappings"),
                    TypeSetFolderPath = Path.Combine(AssemblyDirectory, "CSharp", "TypeSets")
                };
            persistanceFacade = new PersistenceFacade(persistanceFacadeConfiguration);
            _tested = new InputModelHandler();
        }

        

        [Test]
        public void ShouldReturnExpectedResult()
        {
            var templateContent = GetExpectedTemplateContent("Dao.dbtemplate");
            var databaseModel = persistanceFacade.GetDatabaseModelByPersistenceName("OnlyEmployeePayHistorySqlServerDatabaseModel");

            IDatabaseTemplateHandlerInputModel input = new DatabaseTemplateHandlerInputModel()
            {
                DatabaseModel = databaseModel,
                TemplateModels = new List<ITemplateModel>()
                { 
                    new TemplateModel()
                    {
                        TemplatedFilePath = $"{_tested.TableFilePathTemplateWord}Dao.cs",
                        TemplatedFileContent = templateContent,
                    },
                },
                typeMappings = new List<ITypeMapping>(),
            };

            var result = _tested.Process(input);
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            var expected = GetExpectedResultContent("EmployeePayHistoryDao.cs.expected");
            var actual = result.First().Content;
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void Test()
        {
            var templateContent = @"{:TDB:PREPROCESSOR:MAPPING:DECLARE(([->(C#)<-]<=>[[->(hierarchyid)<-]=>[->(Microsoft.SqlServer.Types.SqlHierarchyId)<-],
[->(smallint)<-]=>[->(short)<-],
[->(date)<-]=>[->(DateTime)<-],
[->(geography)<-]=>[->(Microsoft.SqlServer.Types.SqlGeography)<-],
[->(xml)<-]=>[->(System.Xml.XmlDocument)<-],
[->(varchar)<-]=>[->(string)<-],
[->(varbinary)<-]=>[->(byte[])<-],
[->(uniqueidentifier)<-]=>[->(Guid)<-],
[->(tinyint)<-]=>[->(byte)<-],
[->(timestamp)<-]=>[->(byte)<-],
[->(time)<-]=>[->(TimeSpan)<-],
[->(sql_variant)<-]=>[->(object)<-],
[->(smallmoney)<-]=>[->(decimal)<-],
[->(smalldatetime)<-]=>[->(DateTime)<-],
[->(rowversion)<-]=>[->(byte[])<-],
[->(nvarchar)<-]=>[->(string)<-],
[->(numeric)<-]=>[->(decimal)<-],
[->(ntext)<-]=>[->(string)<-],
[->(nchar)<-]=>[->(string)<-],
[->(money)<-]=>[->(decimal)<-],
[->(int)<-]=>[->(int)<-],
[->(image)<-]=>[->(byte[])<-],
[->(float)<-]=>[->(double)<-],
[->(varbinary)<-]=>[->(byte[])<-],
[->(decimal)<-]=>[->(decimal)<-],
[->(datetimeoffset)<-]=>[->(DateTimeOffset)<-],
[->(datetime)<-]=>[->(DateTime)<-],
[->(char)<-]=>[->(string)<-],
[->(bit)<-]=>[->(bool)<-],
[->(binary)<-]=>[->(byte[])<-],
[->(bigint)<-]=>[->(long)<-]
]):PREPROCESSOR:}        protected override {:TDB:TABLE:CURRENT:NAME::}Model ToModel(SqlDataReader dataReader)
        {
            var result = new {:TDB:TABLE:CURRENT:NAME::}Model();
{:TDB:TABLE:COLUMN:FOREACH[             result.{:TDB:FUNCTION:REPLACE({:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}<-:WITH:[ ]:BY:[])::} = ({:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE(C#)::}{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NULL(?):::})(dataReader[""{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}""]{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NULL( is DBNull ? null : dataReader[""{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}""]):::});
]::}            return result;
        }";
            var databaseModel = persistanceFacade.GetDatabaseModelByPersistenceName("OnlyEmployeePayHistorySqlServerDatabaseModel");

            IDatabaseTemplateHandlerInputModel input = new DatabaseTemplateHandlerInputModel()
            {
                DatabaseModel = databaseModel,
                TemplateModels = new List<ITemplateModel>()
                {
                    new TemplateModel()
                    {
                        TemplatedFilePath = $"{_tested.TableFilePathTemplateWord}Dao.cs",
                        TemplatedFileContent = templateContent,
                    },
                },
                typeMappings = new List<ITypeMapping>(),
            };

            var result = _tested.Process(input);
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            var expected = @"        protected override EmployeePayHistoryModel ToModel(SqlDataReader dataReader)
        {
            var result = new EmployeePayHistoryModel();
             result.BusinessEntityID = (int)(dataReader[""BusinessEntityID""]);
             result.RateChangeDate = (DateTime)(dataReader[""RateChangeDate""]);
             result.Rate = (decimal)(dataReader[""Rate""]);
             result.PayFrequency = (byte)(dataReader[""PayFrequency""]);
             result.ModifiedDate = (DateTime)(dataReader[""ModifiedDate""]);
            return result;
        }";
            var actual = result.First().Content;
            Assert.AreEqual(expected, actual);
        }
    }
}
