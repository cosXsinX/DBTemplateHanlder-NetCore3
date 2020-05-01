using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.UnitTests.Handlers
{
    [TestFixture]
    public class TemplateHandlerUnitTests
    {
        private TemplatePreprocessor _tested;

        private ITemplateHandler _templateHandler;

        private IList<ITypeMapping> _typeMappings;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _typeMappings = new List<ITypeMapping>();
            _templateHandler = TemplateHandlerBuilder.Build(_typeMappings);
            _tested = new TemplatePreprocessor(_templateHandler, _typeMappings);
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
        public void InlineMappingLoadTest()
        {
            var firstContext = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE(
[->({envB})<-]<=>[
[->({envAType1})<-]=>[->({envBType1})<-],
[->({envAType2})<-]=>[->({envBType2})<-],
[->({envAType3})<-]=>[->({envBType3})<-]
]
):PREPROCESSOR:}}";

            var secondContext = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE(
[->({envC})<-]<=>[
[->({envAType1})<-]=>[->({envCType1})<-],
[->({envAType2})<-]=>[->({envCType2})<-],
[->({envAType3})<-]=>[->({envCType3})<-]
]
):PREPROCESSOR:}}";

            string preprocessed = $@"{firstContext}

{secondContext}
";
            var preprocessedTemplateModel = new TemplateModelForTest()
            {
                TemplatedFileContent = preprocessed
            };

             _tested.PreProcess(new List<ITemplateModel>(){ preprocessedTemplateModel });

            var expectedPreprocessedResult = $@"


";
            Assert.AreEqual(expectedPreprocessedResult, preprocessedTemplateModel.TemplatedFileContent);
    }

        public class TemplateModelForTest : ITemplateModel
        {
            public string TemplatedFileContent { get; set; }
            public string TemplatedFilePath { get; set ; }

            public ITemplateModel Copy()
            {
                var result = new TemplateModelForTest();
                result.TemplatedFilePath = result.TemplatedFilePath;
                result.TemplatedFileContent = result.TemplatedFileContent;
                return result;
            }
        }

    }
}
