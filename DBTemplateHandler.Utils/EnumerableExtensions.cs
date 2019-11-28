using System;
using System.Linq;
using System.Collections.Generic;

namespace DBTemplateHandler.Utils
{
    public static class EnumerableExtensions 
    {
        public static IEnumerable<Tuple<T,U>> LeftJoin<T,U,K>(this IEnumerable<T> left,IEnumerable<U> rigth,Func<T,K> leftKey, Func<U,K> rightKey)
        {
            var rightByRightKey = rigth.GroupBy(rightKey).ToDictionary(m => m.Key, m => m.ToList());
            var result = left.SelectMany(curLeft =>
            {
                if (rightByRightKey.TryGetValue(leftKey(curLeft), out List<U> value)) return
                     value.Select(curRight => Tuple.Create(curLeft, curRight)).ToList();
                return new List<Tuple<T, U>>() { Tuple.Create(curLeft, default(U)) };
            }).ToList();
            return result;
        }

        public static IEnumerable<Tuple<T, U>> InnerJoin<T, U, K>(this IEnumerable<T> left, IEnumerable<U> rigth, Func<T, K> leftKey, Func<U, K> rightKey)
        {
            var rightByRightKey = rigth.GroupBy(rightKey).ToDictionary(m => m.Key, m => m.ToList());
            var result = left.SelectMany(curLeft =>
                rightByRightKey.TryGetValue(leftKey(curLeft), out List<U> value) ?
                    value.Select(curRight => Tuple.Create(curLeft, curRight)).ToList() : new List<Tuple<T, U>>() {}).ToList();
            return result;
        }
    }
}
