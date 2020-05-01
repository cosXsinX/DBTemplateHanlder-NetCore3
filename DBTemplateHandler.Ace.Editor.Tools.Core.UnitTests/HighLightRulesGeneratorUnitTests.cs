using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.Ace.Editor.Tools.Core.UnitTests
{
    [TestFixture]
    public class HighLightRulesGeneratorUnitTests
    {
        HighLightRulesGenerator _tested;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = TemplateHandlerBuilder.Build(null);
            TemplateContextHandlerRegister register = new TemplateContextHandlerRegister(templateHandlerNew,null);
            _tested = new HighLightRulesGenerator(register);
        }

        [Test]
        public void GetDbTemplateAceMode()
        {
            var result = _tested.GetDbTemplateAceMode();
            WriteInTestContext(result);
        }

        private static void WriteInTestContext(FileModel result)
        {
            TestContext.WriteLine("File name :");
            TestContext.WriteLine(result.FileName);
            TestContext.WriteLine("File content :");
            TestContext.WriteLine(result.Content);
        }
    }
}
