// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Collections {
  public class EnumeratorAsEnumerable<T>: IEnumerable<T> {
    #region [Property: Enumerator]
    private readonly IEnumerator<T> enumerator;

    protected IEnumerator<T> Enumerator {
      get { return this.enumerator; }
    }
    #endregion

    #region [IEnumerable Implementation]
    private bool isEnumeratorReturned;
    public IEnumerator<T> GetEnumerator() {
      if (this.isEnumeratorReturned)
        throw new InvalidOperationException("Enumerate was already returned - this instance is invalid.");

      this.isEnumeratorReturned = true;
      return this.Enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }
    #endregion


    #region [Method: Constructor]
    public EnumeratorAsEnumerable(IEnumerator<T> enumerator) {
      this.enumerator = enumerator;
    }
    #endregion
  }
}
