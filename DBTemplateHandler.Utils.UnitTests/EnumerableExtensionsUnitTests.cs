using NUnit.Framework;
using System;
using DBTemplateHandler.Utils;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Utils.UnitTests
{
    [TestFixture]
    public class EnumerableExtensionsUnitTests
    {
        [Test]
        public void LeftJoinShouldReturnJoinedTupleWhenKeyAreTheSame()
        {
            var key = "Key";
            var leftValue = "left";
            var rightValue = "right";
            Joined left = new Joined() { Key = key, Value = leftValue };
            Joined right = new Joined() { Key = key, Value = rightValue };
            var jointure = (new List<Joined>() { left }).LeftJoin((new List<Joined>() { right }), m => m.Key, m => m.Key);
            Assert.IsNotNull(jointure);
            var jointureAsList = jointure.ToList();
            CollectionAssert.IsNotEmpty(jointureAsList);
            Assert.AreEqual(1, jointureAsList.Count);
            var joined = jointureAsList.First();
            Assert.IsNotNull(joined.Item1);
            Assert.IsNotNull(joined.Item2);
            Assert.AreEqual(key, left.Key);
            Assert.AreEqual(key, right.Key);
            Assert.AreEqual(leftValue, joined.Item1.Value);
            Assert.AreEqual(rightValue, joined.Item2.Value);
        }

        [Test]
        public void LeftJoinShouldReturnJointureWithRightNull()
        {
            var key = "Key";
            var leftValue = "left";
            var rightValue = "right";
            Joined left = new Joined() { Key = key, Value = leftValue };
            Joined right = new Joined() { Key = "otherKey", Value = rightValue };
            var jointure = (new List<Joined>() { left }).LeftJoin((new List<Joined>() { right }), m => m.Key, m => m.Key);
            Assert.IsNotNull(jointure);
            var jointureAsList = jointure.ToList();
            CollectionAssert.IsNotEmpty(jointureAsList);
            Assert.AreEqual(1, jointureAsList.Count);
            var joined = jointureAsList.First();
            Assert.IsNotNull(joined.Item1);
            Assert.IsNull(joined.Item2);
            Assert.AreEqual(key, joined.Item1.Key);
            Assert.AreEqual(leftValue, joined.Item1.Value);
        }

        public class Joined
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

    }
}
