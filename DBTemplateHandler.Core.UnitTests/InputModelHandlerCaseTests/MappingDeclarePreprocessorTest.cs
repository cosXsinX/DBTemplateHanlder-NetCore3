using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.InputModelHandlerCaseTests
{
    [TestFixture]
    public class MappingDeclarePreprocessorTest
    {
        private InputModelHandler inputModelHandler;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            inputModelHandler = new InputModelHandler();
        }

        const string envB = nameof(envB);
        const string envAType1 = nameof(envAType1);
        const string envAType2 = nameof(envAType2);
        const string envAType3 = nameof(envAType3);
        const string envBType1 = nameof(envBType1);
        const string envBType2 = nameof(envBType2);
        const string envBType3 = nameof(envBType3);


        const string envC = nameof(envC);
        const string envCType1 = nameof(envCType1);
        const string envCType2 = nameof(envCType2);
        const string envCType3 = nameof(envCType3);

        [Test]
        public void CrossPreprocessorDeclarationTest()
        {
            const string envBTemplateWhichShouldBeTested = nameof(envBTemplateWhichShouldBeTested);
            const string envCTemplateWhichShouldBeTested = nameof(envCTemplateWhichShouldBeTested);

            IDatabaseTemplateHandlerInputModel inputModel = new InputModelForTest()
            {
                DatabaseModel = new DatabaseModelForTest()
                {
                    Tables = new List<ITableModel>()
                    {
                        new TableModelForTest()
                        {
                            Columns = new List<IColumnModel>()
                            {
                                new ColumnModelForTest()
                                {
                                    Name = $"{envAType1}",
                                    Type = $"{envAType1}",
                                },
                                new ColumnModelForTest()
                                {
                                    Name = $"{envAType2}",
                                    Type = $"{envAType2}",
                                },
                                new ColumnModelForTest()
                                {
                                    Name = $"{envAType3}",
                                    Type = $"{envAType3}",
                                },
                            }
                        }
                    }
                },
                TemplateModels = new List<ITemplateModel>()
                {
                    new TemplateModelForTest()
                    {
                        TemplatedFilePath="MappingDeclarationEnvB",
                        TemplatedFileContent = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE(
[->({envB})<-]<=>[
[->({envAType1})<-]=>[->({envBType1})<-],
[->({envAType2})<-]=>[->({envBType2})<-],
[->({envAType3})<-]=>[->({envBType3})<-]
]
):PREPROCESSOR:}}"
                    },
                    new TemplateModelForTest()
                    {
                        TemplatedFilePath="MappingDeclarationEnvC",
                        TemplatedFileContent = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE(
[->({envC})<-]<=>[
[->({envAType1})<-]=>[->({envCType1})<-],
[->({envAType2})<-]=>[->({envCType2})<-],
[->({envAType3})<-]=>[->({envCType3})<-]
]
):PREPROCESSOR:}}"
                    },
                    new TemplateModelForTest()
                    {
                        TemplatedFilePath= envBTemplateWhichShouldBeTested,
                        TemplatedFileContent = ToTestTemplate(envB),
                    },
                    new TemplateModelForTest()
                    {
                        TemplatedFilePath= envCTemplateWhichShouldBeTested,
                        TemplatedFileContent =ToTestTemplate(envC),
                    }
                },
            };
            var processingResults = inputModelHandler.Process(inputModel);
            Assert.IsNotNull(processingResults);
            CollectionAssert.IsNotEmpty(processingResults);
            var paths = processingResults.Select(m => m.Path).ToList();
            CollectionAssert.AllItemsAreUnique(paths);
            CollectionAssert.Contains(paths,envBTemplateWhichShouldBeTested);
            CollectionAssert.Contains(paths,envCTemplateWhichShouldBeTested);
            var ContentbyPath = processingResults.ToDictionary(m => m.Path, m => m.Content);
            var envBContent = ContentbyPath[envBTemplateWhichShouldBeTested];
            Assert.AreEqual(@$"{envAType1}->{envBType1}
{envAType2}->{envBType2}
{envAType3}->{envBType3}
", envBContent);

            var envCContent = ContentbyPath[envCTemplateWhichShouldBeTested];
            Assert.AreEqual(@$"{envAType1}->{envCType1}
{envAType2}->{envCType2}
{envAType3}->{envCType3}
", envCContent);
        }

        private string ToTestTemplate(string destinationEnv)
        {
            return $@"{{:TDB:TABLE:FOREACH[{{:TDB:TABLE:COLUMN:FOREACH[{{:TDB:TABLE:COLUMN:FOREACH:CURRENT:NAME::}}->{{:TDB:TABLE:COLUMN:FOREACH:CURRENT:CONVERT:TYPE({destinationEnv})::}}
]::}}]::}}";
        }
    }
}
