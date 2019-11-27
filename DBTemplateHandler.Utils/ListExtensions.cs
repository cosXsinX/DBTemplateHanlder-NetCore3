﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace DBTemplateHandler.Utils
{
    public static class ListExtensions 
    {
        public static IList<Tuple<T,U>> LeftJoin<T,U>(this List<T> right,IEnumerable<U> left,Func<T,dynamic> rightKey, Func<U,dynamic> leftKey)
        {
            var rightByRightKey = right.GroupBy(rightKey).ToDictionary(m => m.Key, m => m.ToList());
            var result = left.SelectMany(m => 
            rightByRightKey.TryGetValue(leftKey(m),out List<T> value) ? 
                value.Select(v => Tuple.Create(v,m)).ToList() : new List<Tuple<T, U>>() { Tuple.Create(default(T),m )}).ToList();
            return result;
        }

        public static IList<Tuple<T, U>> InnerJoin<T, U>(this List<T> right, IEnumerable<U> left, Func<T, dynamic> rightKey, Func<U, dynamic> leftKey)
        {
            var rightByRightKey = right.GroupBy(rightKey).ToDictionary(m => m.Key, m => m.ToList());
            var result = left.SelectMany(m =>
            rightByRightKey.TryGetValue(leftKey(m), out List<T> value) ?
                value.Select(v => Tuple.Create(v, m)).ToList() : new List<Tuple<T, U>>() {}).ToList();
            return result;
        }
    }
}
