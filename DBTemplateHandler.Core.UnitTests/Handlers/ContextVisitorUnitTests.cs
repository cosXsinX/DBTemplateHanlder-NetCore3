using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using NUnit.Framework;
using System;
using System.Linq;

namespace DBTemplateHandler.Core.UnitTests.Handlers
{
    [TestFixture]
    public class ContextVisitorUnitTests
    {
        private ITemplateHandler templateHandlerNew;
        private ContextVisitor<ITemplateContextHandler> _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            templateHandlerNew = TemplateHandlerBuilder.Build(null);
            ITemplateContextHandlerPackageProvider<ITemplateContextHandler> templateContextHandlerPackageProvider = 
                new TemplateContextHandlerPackageProvider<ITemplateContextHandler>(templateHandlerNew,null);
            _tested = new ContextVisitor<ITemplateContextHandler>(templateContextHandlerPackageProvider);

        }

        [Test]
        public void ShouldReturnTwoContextesWithNoChilds()
        {
            DatabaseNameDatabaseContextHandler databaseNameDatabaseContextHandler = new DatabaseNameDatabaseContextHandler(templateHandlerNew);
            TableNameTableContextHandler tableNameTableContextHandler = new TableNameTableContextHandler(templateHandlerNew);
            var results = _tested.ExtractAllContextUntilDepth($"{databaseNameDatabaseContextHandler.Signature}-{tableNameTableContextHandler.Signature}",0);
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

        [Test]
        public void ShouldReturnOnlyOneContextWhenTwoStartContextDelimiterAndOnlyOneEndContext()
        {
            DatabaseNameDatabaseContextHandler databaseNameDatabaseContextHandler = new DatabaseNameDatabaseContextHandler(templateHandlerNew);
            TableNameTableContextHandler tableNameTableContextHandler = new TableNameTableContextHandler(templateHandlerNew);
            var results = _tested.ExtractAllContextUntilDepth($"{databaseNameDatabaseContextHandler.Signature}-{tableNameTableContextHandler.StartContext}", 0);
            Assert.IsNotNull(results);
            var resultAsList = results.ToList();
            CollectionAssert.IsNotEmpty(resultAsList);
            Assert.AreEqual(1, resultAsList.Count);
            var first = resultAsList[0];
            Assert.IsNotNull(first.current);
            Assert.IsNotNull(first.childs);
            CollectionAssert.IsEmpty(first.childs);
            Assert.AreEqual(first.current.StartContextDelimiter, databaseNameDatabaseContextHandler.StartContext);
            Assert.AreEqual(first.current.InnerContent, String.Empty);
            Assert.AreEqual(first.current.EndContextDelimiter, databaseNameDatabaseContextHandler.EndContext);
        }


        [Test]
        public void ShouldReturnOnlyOneContextWithTheSecondNested()
        {
            ForEachTableDatabaseContextHandler forEachTableDatabaseContextHandler = new ForEachTableDatabaseContextHandler(templateHandlerNew);
            TableNameTableContextHandler tableNameTableContextHandler = new TableNameTableContextHandler(templateHandlerNew);
            var results = _tested.ExtractAllContextUntilDepth($"{forEachTableDatabaseContextHandler.StartContext}{tableNameTableContextHandler.StartContext}{tableNameTableContextHandler.EndContext}{forEachTableDatabaseContextHandler.EndContext}", 1);
            Assert.IsNotNull(results);
            var resultAsList = results.ToList();
            CollectionAssert.IsNotEmpty(resultAsList);
            Assert.AreEqual(1, resultAsList.Count);
            var first = resultAsList[0];
            Assert.IsNotNull(first.current);
            Assert.IsNotNull(first.childs);
            CollectionAssert.IsNotEmpty(first.childs);
            Assert.AreEqual(first.current.StartContextDelimiter, forEachTableDatabaseContextHandler.StartContext);
            Assert.AreEqual(first.current.InnerContent, $"{tableNameTableContextHandler.StartContext}{tableNameTableContextHandler.EndContext}");
            Assert.AreEqual(first.current.EndContextDelimiter, forEachTableDatabaseContextHandler.EndContext);
            Assert.AreEqual(1, first.childs.Count);
            var firstChild = first.childs.First();
            Assert.IsNotNull(firstChild.current);
            Assert.IsNotNull(firstChild.childs);
            Assert.AreEqual(firstChild.current.StartContextDelimiter, tableNameTableContextHandler.StartContext);
            Assert.AreEqual(firstChild.current.InnerContent, String.Empty);
            Assert.AreEqual(firstChild.current.EndContextDelimiter, tableNameTableContextHandler.EndContext);
        }

        [Test]
        public void ShouldReturnTwoNestedContextWhenThereAreTwoNestedContextDeclaration()
        {
            ForEachTableDatabaseContextHandler forEachTableDatabaseContextHandler = new ForEachTableDatabaseContextHandler(templateHandlerNew);
            var results = _tested.ExtractAllContextUntilDepth(
                $"{forEachTableDatabaseContextHandler.StartContext}{forEachTableDatabaseContextHandler.StartContext}{forEachTableDatabaseContextHandler.EndContext}{forEachTableDatabaseContextHandler.EndContext}", 1);
            Assert.IsNotNull(results);
            var resultAsList = results.ToList();
            CollectionAssert.IsNotEmpty(resultAsList);
            Assert.AreEqual(1, resultAsList.Count);
            var first = resultAsList[0];
            Assert.IsNotNull(first.current);
            Assert.IsNotNull(first.childs);
            CollectionAssert.IsNotEmpty(first.childs);
            Assert.AreEqual(first.current.StartContextDelimiter, forEachTableDatabaseContextHandler.StartContext);
            Assert.AreEqual(first.current.InnerContent, $"{forEachTableDatabaseContextHandler.StartContext}{forEachTableDatabaseContextHandler.EndContext}");
            Assert.AreEqual(first.current.EndContextDelimiter, forEachTableDatabaseContextHandler.EndContext);
            Assert.AreEqual(1, first.childs.Count);
            Assert.AreEqual(1, first.childs.Count);
            var firstChild = first.childs.First();
            Assert.IsNotNull(firstChild.current);
            Assert.IsNotNull(firstChild.childs);
            Assert.AreEqual(firstChild.current.StartContextDelimiter, forEachTableDatabaseContextHandler.StartContext);
            Assert.AreEqual(firstChild.current.InnerContent, String.Empty);
            Assert.AreEqual(firstChild.current.EndContextDelimiter, forEachTableDatabaseContextHandler.EndContext);
        }
    }
}
