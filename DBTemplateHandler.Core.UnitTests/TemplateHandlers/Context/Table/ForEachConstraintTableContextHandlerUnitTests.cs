using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Constraints;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using DBTemplateHandler.Core.UnitTests.ModelImplementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Context.Table
{
    [TestFixture]
    public class ForEachConstraintTableContextHandlerUnitTests
    {
        ConstraintNameConstraintContextHandler constraintNameConstraintContextHandler;
        ForEachConstraintTableContextHandler _tested;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var templateHandler = TemplateHandlerBuilder.Build(null);
            _tested = new ForEachConstraintTableContextHandler(templateHandler);
            constraintNameConstraintContextHandler = new ConstraintNameConstraintContextHandler(templateHandler);
        }


        [Test]
        public void ShouldReturnAccurateStartContextValue()
        {
            Assert.AreEqual("{:TDB:TABLE:CONSTRAINT:FOREACH[", _tested.StartContext);
        }

        [Test]
        public void ShouldReturnAccurateEndContextValue()
        {
            Assert.AreEqual("]::}", _tested.EndContext);
        }

        [Test]
        public void isStartContextAndEndContextAnEntireWordShouldReturnFalse()
        {
            Assert.IsFalse(_tested.isStartContextAndEndContextAnEntireWord);
        }

        [Test]
        public void ShouldReturnAccurateContextActionDescription()
        {
            Assert.AreEqual("Is replaced by the intern context as many time as there is constraint on the table", _tested.ContextActionDescription);
        }


        [Test]
        public void ShouldThrowAnExceptionWhenStringContextIsNull()
        {
            string StringContext = null;
            Assert.Throws<Exception>(() => _tested.ProcessContext(StringContext, new ProcessorDatabaseContext() { ForeignKeyConstraint = new ForeignKeyConstraintModel() { } })
                , $"The provided {nameof(StringContext)} is null");
        }

        [Test]
        public void ShouldThrowANExceptionWhenTableModelIsNull()
        {
            string StringContext = "Hello World !";
            Assert.Throws<ArgumentNullException>(() => _tested.ProcessContext(StringContext,
                new ProcessorDatabaseContext() { Table = null }));
        }

        [Test]
        public void ShouldThrowAnExceptionWhenConstrainModelListIsNullInTableModel()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext() { Table = new TableModelForTest() { ForeignKeyConstraints = null } };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldThrowAnExceptionWhenAtLeastOneConstrainModelInConstrainsFromTableModelIsNull()
        {
            Assert.Throws<Exception>(() =>
            {
                var databaseContext = new ProcessorDatabaseContext()
                {
                    Table =
                    new TableModelForTest() { ForeignKeyConstraints = new List<IForeignKeyConstraintModel>() { new ForeignKeyConstraintModel() { }, null } }
                };
                _tested.ProcessContext("Hello World !", databaseContext);
            });
        }

        [Test]
        public void ShouldReturnEveryConstraintNamePresentInTheTable()
        {
            var firstConstraintName = "firstConstraintName";
            var secondConstraintName = "secondConstraintName";
            var databaseContext = new ProcessorDatabaseContext()
            {
                Table =
                    new TableModelForTest()
                    {
                        ForeignKeyConstraints = new List<IForeignKeyConstraintModel>()
                    {
                        new ForeignKeyConstraintModel() { ConstraintName = firstConstraintName },
                        new ForeignKeyConstraintModel() { ConstraintName = secondConstraintName },
                    }
                }
            };
            var templateContext = $"{_tested.StartContext}{constraintNameConstraintContextHandler.Signature},{_tested.EndContext}";
            var actual = _tested.ProcessContext(templateContext, databaseContext);
            var expected = $"{firstConstraintName},{secondConstraintName},";
            Assert.AreEqual(expected, actual);
        }

    }
}
