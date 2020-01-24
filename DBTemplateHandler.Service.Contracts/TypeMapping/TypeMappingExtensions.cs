using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Service.Contracts.TypeMapping
{
    public static class TypeMappingExtensions
    {
        public static IEnumerable<ITypeMapping> MergeAndAttachTypeMappingItems(this IEnumerable<ITypeMapping> overwrittens, IEnumerable<ITypeMapping> overwrittings)
        {
            Func<ITypeMapping, string> keyFunc = m => $"{m.SourceTypeSetName}-{m.DestinationTypeSetName}";
            var overwrittenWithKeys = overwrittens.Select(overwritten => (key: keyFunc(overwritten), overwritten)).ToList();
            var overwrittingWithKeys = overwrittings.Select(overwritting => (key: keyFunc(overwritting), overwritting)).ToList();
            var overwrittenKeySet = new HashSet<string>(overwrittenWithKeys.Select(m => m.key).Distinct());
            var overwrittingKeySet = new HashSet<string>(overwrittingWithKeys.Select(m => m.key).Distinct());
            var notContainedInOverwritten = overwrittingWithKeys.Where(m => !overwrittenKeySet.Contains(m.key)).Select(m => m.overwritting);
            
            var containedInOverwritten = overwrittingWithKeys.Where(m => overwrittingKeySet.Contains(m.key))
                .GroupBy(m => m.key, m => m.overwritting).ToDictionary(m => m.Key, m => m.First());
            var containedInOverwritting = overwrittenWithKeys.Where(m => overwrittenKeySet.Contains(m.key))
                .GroupBy(m => m.key, m => m.overwritten).Select(m => (key : m.Key, overwritten : m.First())).ToList();

            containedInOverwritting.ForEach(m => m.overwritten.TypeMappingItems.OverwriteWith(containedInOverwritten[m.key].TypeMappingItems));
            var actuallyOverwriten = containedInOverwritting.Select(m => m.overwritten);
            return overwrittens.Concat(notContainedInOverwritten);
        }

        public static IEnumerable<ITypeMappingItem> OverwriteWith(this IEnumerable<ITypeMappingItem> overwrittens, IEnumerable<ITypeMappingItem> overwrittings)
        {
            Func<ITypeMappingItem, string> keyFunc = m => $"{m.SourceType}-{m.DestinationType}";
            var overwrittingByKey = new HashSet<string>(overwrittings.Select(keyFunc).Distinct()) ;
            var overwrittenWithKey = overwrittens.Select(typeMappingItem => (key : keyFunc(typeMappingItem), typeMappingItem));
            var notOverwritten = overwrittenWithKey.Where(m => !overwrittingByKey.Contains(m.key)).Select(m => m.typeMappingItem);
            return notOverwritten.Concat(overwrittens);
        }
    }
}
