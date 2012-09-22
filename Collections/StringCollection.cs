// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Collections {
  public class StringCollection: Collection<string> {
    #region [Method: Constructor]
    public StringCollection() {}

    public StringCollection(IList<string> wrappedList): base(wrappedList) {}
    #endregion

    #region [Method: Contains, IndexOf]
    public int IndexOf(string item, StringComparison comparison = StringComparison.InvariantCulture) {
      if (comparison == StringComparison.InvariantCulture)
        return base.IndexOf(item);

      for (int i = 0; i < this.Items.Count; i++) {
        if (this.Items[i].Equals(item, comparison))
          return i;
      }

      return -1;
    }

    public bool Contains(string item, StringComparison comparison = StringComparison.InvariantCulture) {
      return (this.IndexOf(item, comparison) > -1);
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return string.Join(", ", this.Items);
    }
    #endregion
  }
}
