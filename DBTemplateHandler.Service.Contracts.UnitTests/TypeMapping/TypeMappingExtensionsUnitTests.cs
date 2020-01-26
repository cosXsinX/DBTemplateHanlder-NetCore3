using DBTemplateHandler.Service.Contracts.TypeMapping;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Service.Contracts.UnitTests.TypeMapping
{
    [TestFixture]
    public class TypeMappingExtensionsUnitTests
    {
        [Test]
        public void MergeAndAttachTypeMappingItems()
        {
            IList<ITypeMapping> overwrittens = new List<ITypeMapping>() {
                new TypeMappingForTest()
                {
                    SourceTypeSetName = "Sql Server",
                    DestinationTypeSetName = "C#",
                    TypeMappingItems = new List<ITypeMappingItem>()
                    {
                        new TypeMappingItemForTest()
                        {
                            SourceType = "INT",
                            DestinationType = "int",
                        }
                    }
                }};

            IList<ITypeMapping> overwritting = new List<ITypeMapping> { new TypeMappingForTest()
            {
                SourceTypeSetName = "Sql Server",
                DestinationTypeSetName = "VBA",
                TypeMappingItems = new List<ITypeMappingItem>()
                {
                    new TypeMappingItemForTest()
                    {
                        SourceType = "INT",
                        DestinationType = "Int",
                    }
                }
            } };
            var result = overwrittens.MergeAndAttachTypeMappingItems(overwritting);
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result);
            var expectedNormalizedResult = ToTypeMappingStringItemEquivalent(overwrittens.Concat(overwritting).ToList());
            var actualNormalizedResult = ToTypeMappingStringItemEquivalent(result.ToList());
            CollectionAssert.AreEquivalent(expectedNormalizedResult, actualNormalizedResult);
        }

        private IList<string> ToTypeMappingStringItemEquivalent(IList<ITypeMapping> typeMappings)
        {
            var typeMappingAndTypeMappingItem = typeMappings
                .Where(typeMapping => typeMapping != null)
                .SelectMany(typeMapping => typeMapping.TypeMappingItems.Select(typeMappingItem => (typeMapping, typeMappingItem))).ToList();
            var result = typeMappingAndTypeMappingItem.
                Select(m => $"{m.typeMapping.SourceTypeSetName}->{m.typeMapping.DestinationTypeSetName}:{m.typeMappingItem.SourceType}->{m.typeMappingItem.DestinationType}").
                ToList();
            return result;
        }


        public class TypeMappingForTest : ITypeMapping
        {
            public string DestinationTypeSetName { get; set; }
            public string SourceTypeSetName { get; set; }
            public IList<ITypeMappingItem> TypeMappingItems { get; set; }
        }

        public class TypeMappingItemForTest : ITypeMappingItem
        {
            public string DestinationType { get;set; }
            public string SourceType { get;set; }
        }
    }
}
