using System;
using System.Collections;
using System.Collections.Generic;

namespace Terraria.Plugins.CoderCow {
  public static class CollectionEx {
    public static bool TryGetKey<TKey,TValue>(this IDictionary dictionary, TValue value, out TKey key) {
      foreach (KeyValuePair<TKey,TValue> pair in dictionary) {
        if (pair.Value.Equals(value)) {
          key = pair.Key;
          return true;
        }
      }

      key = default(TKey);
      return false;
    }
  }
}
