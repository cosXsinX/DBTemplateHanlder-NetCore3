﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Columns
{
    [TestFixture]
    public class ColumnValueMaxSizeColumnContextHandlerUnitTests
    {
        private ColumnValueMaxSizeColumnContextHandler _tested;
        private Random _random;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandlerNew = TemplateHandlerBuilder.Build(null);
            _tested = new ColumnValueMaxSizeColumnContextHandler(templateHandlerNew);
            _random = new Random();
        }

        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:TABLE:COLUMN:FOREACH:CURRENT:MAX:SIZE", _tested.StartContext);
        }

        [Test]
        public void ShouldReturnAccurateEndContextValue()
        {
            Assert.AreEqual("::}", _tested.EndContext);
        }

        [Test]
        public void isStartContextAndEndContextAnEntireWordShouldReturnTrue()
        {
            Assert.IsTrue(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ShouldReturnAccurateContextActionDescription()
        {
            Assert.AreEqual("Is replaced by the current column value max/length size", _tested.ContextActionDescription);
        }


        [Test]
        public void ShouldThrowAnArgumentNullExceptionWhenDatabaseContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext("Hello world", null));
        }

        [Test]
        public void IsStartContextAndEndContextAnEntireWordShouldReturnTrue()
        {
            Assert.IsTrue(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { } })
                , $"The provided {nameof(StringContext)} is null");
        }


        [Test]
        public void ShouldThrowANExceptionWhenDatabaseModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Column = null };
                _tested.ProcessContext("Hello World !", databaseContext);
            }, $"The {nameof(ProcessorDatabaseContext.Column)} is not set");
        }

        [Test]
        public void ShouldThrowAnExceptionWhenThereIsNoValueMaxSizePlaceHolder()
        {
            int valueMaxSize = _random.Next();
            string StringContext = $"{_tested.StartContext}I Should not be here{_tested.EndContext}";
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { ValueMaxSize = valueMaxSize } };
                _tested.ProcessContext(StringContext,databaseContext);
            }, "Exception control no more satified : Expected exception message There is a problem with the provided StringContext :'" + StringContext + "' to the suited word '" + _tested.Signature + "'");
        }

        [Test]
        public void ShouldReplaceTheValueMaxSizeSignatureByValueMaxSizeValue()
        {
            int valueMaxSize = _random.Next();
            string StringContext = _tested.Signature;
            string expected = $"{valueMaxSize}";
            var databaseContext = new ProcessorDatabaseContext() { Column = new ColumnModelForTest() { ValueMaxSize = valueMaxSize } };
            var actual = _tested.ProcessContext(StringContext,databaseContext);
            Assert.AreEqual(expected, actual);
        }
    }
}
