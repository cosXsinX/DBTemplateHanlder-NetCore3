using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.UnitTests.Handlers
{
    [TestFixture]
    public class ContextVisitorUnitTests
    {
        private TemplateHandlerNew templateHandlerNew;
        private ContextVisitor<ITemplateContextHandler> _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            templateHandlerNew = new TemplateHandlerNew(null);
            ITemplateContextHandlerPackageProvider<ITemplateContextHandler> templateContextHandlerPackageProvider = new TemplateContextHandlerPackageProvider<ITemplateContextHandler>(templateHandlerNew,null);
            _tested = new ContextVisitor<ITemplateContextHandler>(templateContextHandlerPackageProvider);

        }

        [Test]
        public void ShouldReturnTwoContextesWithNoChilds()
        {
            DatabaseNameDatabaseContextHandler databaseNameDatabaseContextHandler = new DatabaseNameDatabaseContextHandler(templateHandlerNew);
            TableNameTableContextHandler tableNameTableContextHandler = new TableNameTableContextHandler(templateHandlerNew);
            var results = _tested.ExtractAllContext($"{databaseNameDatabaseContextHandler.Signature}-{tableNameTableContextHandler.Signature}");
            Assert.IsNotNull(results);
            var resultAsList = results.ToList();
            CollectionAssert.IsNotEmpty(resultAsList);
            Assert.AreEqual(2, resultAsList.Count);
            var first = resultAsList[0];
            Assert.IsNotNull(first.current);
            Assert.IsNotNull(first.childs);
            CollectionAssert.IsEmpty(first.childs);
            Assert.AreEqual(first.current.StartContextDelimiter,  databaseNameDatabaseContextHandler.StartContext);
            Assert.AreEqual(first.current.InnerContent, String.Empty);
            Assert.AreEqual(first.current.EndContextDelimiter, databaseNameDatabaseContextHandler.EndContext);
            var second = resultAsList[1];
            Assert.IsNotNull(second.current);
            Assert.IsNotNull(second.childs);
            CollectionAssert.IsEmpty(second.childs);
            Assert.AreEqual(second.current.StartContextDelimiter, tableNameTableContextHandler.StartContext);
            Assert.AreEqual(second.current.InnerContent, String.Empty);
            Assert.AreEqual(second.current.EndContextDelimiter, tableNameTableContextHandler.EndContext);
        }

    }
}
