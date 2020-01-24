using DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.PreprocessorDeclarations
{
    [TestFixture]
    public class MappingDeclarePreprcessorContextHandlerUnitTests
    {
        private MappingDeclarePreprcessorContextHandlerForTest _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _tested = new MappingDeclarePreprcessorContextHandlerForTest(new TemplateHandlerNew(null));
        }

        [Test]
        public void Test()
        {
          var processed = @"{:TDB:PREPROCESSOR:MAPPING:DECLARE([->(DESTINATION_ENV_B_NAME)<-]<=>[
[->(TYPE1_ENV_A)<-]=>[->(TYPE1_ENV_B)<-],
[->(TYPE2_ENV_A)<-]=>[->(TYPE2_ENV_B)<-],
[->(TYPE3_ENV_A)<-]=>[->(TYPE3_ENV_B)<-]
]):PREPROCESSOR:}";
            _tested.processContext(processed);
            Assert.IsNotNull(_tested.TemplateHandlerNew);

            Assert.IsNotNull(_tested.TemplateHandlerNew.TypeMappings);
            CollectionAssert.IsNotEmpty(_tested.TemplateHandlerNew.TypeMappings);
        }

        

    }
    public class MappingDeclarePreprcessorContextHandlerForTest : MappingDeclarePreprcessorContextHandler
    {
        public MappingDeclarePreprcessorContextHandlerForTest(TemplateHandlerNew templateHandlerNew)
            : base(templateHandlerNew)
        {
        }

        public new TemplateHandlerNew TemplateHandlerNew { get => base.TemplateHandlerNew; }
    }
}
