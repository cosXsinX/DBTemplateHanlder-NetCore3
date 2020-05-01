using DBTemplateHandler.Core.TemplateHandlers.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DBTemplateHandler.Core.TemplateHandlers.Utilities.StringUtilities;

namespace DBTemplateHandler.Core.UnitTests.TemplateHandlers.Utilities
{
    [TestFixture]
    public class StringUtilitiesUnitTests
    {
        [Test]
        public void getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence()
        {
            string submittedString = "Hello I am the best and I am the most wonderful";
            string searchedWord = "am";
            string actual = StringUtilities.
                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(submittedString, searchedWord);
            string expected = "Hello I ";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence()
        {
            string submittedString = "Hello I am the best and I am the most wonderful";
            string searchedWord = "am";
            string actual = StringUtilities.
                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(submittedString, searchedWord);
            string expected = " the best and I am the most wonderful";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ChainShouldBeAccurate()
        {
            var before = new List<string>() { "Hello", "Ola", "Dziendobry" };
            var after = new List<string>() { "Bondie", "Hallo"};
            var actual = StringUtilities.Chain(before, after);
            var expected = new List<string>() { "Hello", "Bondie", "Ola", "Hallo", "Dziendobry" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SplitShouldBeAccurate()
        {
            var indexOffset = 5;
            var splitters = new List<StartAndEndIndexSplitter>()
            {
                new StartAndEndIndexSplitter()
                {
                    StartIndex = indexOffset,
                    EndIndex = indexOffset*2,
                },
                new StartAndEndIndexSplitter()
                {
                    StartIndex = indexOffset*3,
                    EndIndex = indexOffset*4,
                }
            };

            string firstExpectedElement =  String.Join(string.Empty,Enumerable.Repeat("a", indexOffset));
            string secondRemovedElement = String.Join(string.Empty, Enumerable.Repeat("b", indexOffset));
            string thirdExpectedElement = String.Join(string.Empty, Enumerable.Repeat("c", indexOffset));
            string fourthExpectedElement = String.Join(string.Empty, Enumerable.Repeat("d", indexOffset));
            string fifthExpectedElement = String.Join(string.Empty, Enumerable.Repeat("e", indexOffset));
            var splitted = string.Join(string.Empty, new[] { firstExpectedElement, secondRemovedElement, thirdExpectedElement, fourthExpectedElement, fifthExpectedElement }) ;
            var actual = StringUtilities.Split(splitted, splitters).ToList();
            Assert.AreEqual(new[] { firstExpectedElement, thirdExpectedElement, fifthExpectedElement }, actual);
        }
    }
}
