using DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System.Linq;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.PreprocessorDeclarations
{
    [TestFixture]
    public class MappingDeclarePreprcessorContextHandlerUnitTests
    {
       
        const string envB = nameof(envB);
        const string envAType1 = nameof(envAType1);
        const string envAType2 = nameof(envAType2);
        const string envAType3 = nameof(envAType3);
        const string envBType1 = nameof(envBType1);
        const string envBType2 = nameof(envBType2);
        const string envBType3 = nameof(envBType3);

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldReturnAccurateMapping(bool endItemEndWithComa)
        {
            MappingDeclarePreprcessorContextHandlerForTest _tested = new MappingDeclarePreprcessorContextHandlerForTest(TemplateHandlerBuilder.Build(null));
            var processed = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE(
[->({envB})<-]<=>[
[->({envAType1})<-]=>[->({envBType1})<-],
[->({envAType2})<-]=>[->({envBType2})<-],
[->({envAType3})<-]=>[->({envBType3})<-]{(endItemEndWithComa ? "," : string.Empty)}
]
):PREPROCESSOR:}}";
            var result = _tested.processContext(processed);
            Assert.IsNotNull(_tested.TemplateHandlerNew);
            Assert.IsNotNull(_tested.TemplateHandlerNew.TypeMappings);
            var typeMappings = _tested.TemplateHandlerNew.TypeMappings;
            CollectionAssert.IsNotEmpty(typeMappings);
            Assert.AreEqual(1, typeMappings.Count);
            var typeMapping = typeMappings.Single();
            Assert.AreEqual(typeMapping.DestinationTypeSetName, envB);
            Assert.IsNotNull(typeMapping.TypeMappingItems);
            CollectionAssert.IsNotEmpty(typeMapping.TypeMappingItems);
            CollectionAssert.AreEquivalent(
                typeMapping.TypeMappingItems.Select(m => $"{m.SourceType}-{m.DestinationType}"),
                new[] {
                    $"{envAType1}-{envBType1}",
                    $"{envAType2}-{envBType2}",
                    $"{envAType3}-{envBType3}",
                });
            Assert.AreEqual(string.Empty, result);
        }

        [TestCase]
        public void ShouldOneLineDeclarationWork()
        {
            MappingDeclarePreprcessorContextHandlerForTest _tested = new MappingDeclarePreprcessorContextHandlerForTest(TemplateHandlerBuilder.Build(null));
            var processed = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE([->({envB})<-]<=>[[->({envAType1})<-]=>[->({envBType1})<-]]):PREPROCESSOR:}}";
            var result = _tested.processContext(processed); Assert.IsNotNull(_tested.TemplateHandlerNew);
            Assert.IsNotNull(_tested.TemplateHandlerNew.TypeMappings);
            var typeMappings = _tested.TemplateHandlerNew.TypeMappings; CollectionAssert.IsNotEmpty(typeMappings);
            Assert.AreEqual(1, typeMappings.Count);
            var typeMapping = typeMappings.Single();
            Assert.AreEqual(typeMapping.DestinationTypeSetName, envB);
            Assert.IsNotNull(typeMapping.TypeMappingItems);
            CollectionAssert.IsNotEmpty(typeMapping.TypeMappingItems);
            CollectionAssert.AreEquivalent(
                typeMapping.TypeMappingItems.Select(m => $"{m.SourceType}-{m.DestinationType}"),
                new[] {
                    $"{envAType1}-{envBType1}",
                });
            Assert.AreEqual(string.Empty, result);
        }
        [TestCase]
        public void ShouldOneLineDeclarationWithTwoTypeMappingWork()
        {
            MappingDeclarePreprcessorContextHandlerForTest _tested = new MappingDeclarePreprcessorContextHandlerForTest(TemplateHandlerBuilder.Build(null));
            var processed = $@"{{:TDB:PREPROCESSOR:MAPPING:DECLARE([->({envB})<-]<=>[[->({envAType1})<-]=>[->({envBType1})<-],[->({envAType2})<-]=>[->({envBType2})<-]]):PREPROCESSOR:}}";
            var result = _tested.processContext(processed); Assert.IsNotNull(_tested.TemplateHandlerNew);
            Assert.IsNotNull(_tested.TemplateHandlerNew.TypeMappings);
            var typeMappings = _tested.TemplateHandlerNew.TypeMappings; CollectionAssert.IsNotEmpty(typeMappings);
            Assert.AreEqual(1, typeMappings.Count);
            var typeMapping = typeMappings.Single();
            Assert.AreEqual(typeMapping.DestinationTypeSetName, envB);
            Assert.IsNotNull(typeMapping.TypeMappingItems);
            CollectionAssert.IsNotEmpty(typeMapping.TypeMappingItems);
            CollectionAssert.AreEquivalent(
                typeMapping.TypeMappingItems.Select(m => $"{m.SourceType}-{m.DestinationType}"),
                new[] {
                    $"{envAType1}-{envBType1}",
                    $"{envAType2}-{envBType2}",
                });
            Assert.AreEqual(string.Empty, result);
        }
    }


    public class MappingDeclarePreprcessorContextHandlerForTest : MappingDeclarePreprcessorContextHandler
    {
        public MappingDeclarePreprcessorContextHandlerForTest(ITemplateHandler templateHandlerNew)
            : base(templateHandlerNew)
        {
        }

        public new ITemplateHandler TemplateHandlerNew { get => base.TemplateHandler; }
    }
}
