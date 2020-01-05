using System;
using System.Collections;
using System.Collections.Generic;

namespace Terraria.Plugins.Common {
  public static class CollectionEx {
    public static void AddRangeWithoutClones(this IList list, IEnumerable enumerable) {
      if (list == null) throw new ArgumentNullException();
      if (enumerable == null) throw new ArgumentNullException();

      foreach (object item in enumerable) {
        if (!list.Contains(item))
          list.Add(item);
      }
    }

    public static bool TryGetKey<TKey,TValue>(this IDictionary dictionary, TValue value, out TKey key) {
      foreach (DictionaryEntry pair in dictionary) {
        if (pair.Value.Equals(value)) {
          key = (TKey)pair.Key;
          return true;
        }
      }
      
      key = default(TKey);
      return false;
    }
  }
}
