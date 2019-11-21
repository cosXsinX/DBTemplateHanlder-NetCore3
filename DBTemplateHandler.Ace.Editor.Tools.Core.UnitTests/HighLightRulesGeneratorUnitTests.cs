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
            InputModelHandler handler = new InputModelHandler();
            TemplateContextHandlerRegister register = new TemplateContextHandlerRegister();
            _tested = new HighLightRulesGenerator(handler, register);
        }

        [Test]
        public void GetHighlightModelTest()
        {
            var result = _tested.GetHighlightModel();
            TestContext.WriteLine("File name :");
            TestContext.WriteLine(result.FileName);
            TestContext.WriteLine("File content :");
            TestContext.WriteLine(result.Content);
        }

    }
}
