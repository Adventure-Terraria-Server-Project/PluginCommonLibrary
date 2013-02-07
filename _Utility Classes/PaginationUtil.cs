// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public class PaginationUtil {
    #region [Property: IncludeHeader]
    private bool includeHeader;

    public bool IncludeHeader {
      get { return this.includeHeader; }
      set { this.includeHeader = value; }
    }
    #endregion

    #region [Property: HeaderFormat]
    private string headerFormat;

    public string HeaderFormat {
      get { return this.headerFormat; }
      set {
        Contract.Requires<ArgumentNullException>(value != null);
        this.headerFormat = value;
      }
    }
    #endregion

    #region [Property: HeaderTextColor]
    private Color headerTextColor;

    public Color HeaderTextColor {
      get { return this.headerTextColor; }
      set { this.headerTextColor = value; }
    }
    #endregion

    #region [Property: NothingToDisplayString]
    private string nothingToDisplayString;

    public string NothingToDisplayString {
      get { return this.nothingToDisplayString; }
      set { this.nothingToDisplayString = value; }
    }
    #endregion

    #region [Property: LineFormatter]
    private Func<object, string> lineFormatter;

    public Func<object,string> LineFormatter {
      get { return this.lineFormatter; }
      set { this.lineFormatter = value; }
    }
    #endregion

    #region [Property: LineTextColor]
    private Color lineTextColor;

    public Color LineTextColor {
      get { return this.lineTextColor; }
      set { this.lineTextColor = value; }
    }
    #endregion

    #region [Property: MaxLinesPerPage]
    private int maxLinesPerPage;

    public int MaxLinesPerPage {
      get { return this.maxLinesPerPage; }
      set {
        Contract.Requires<ArgumentException>(value > 0);
        this.maxLinesPerPage = value;
      }
    }
    #endregion

    #region [Property: PageLimit]
    private int pageLimit;

    public int PageLimit {
      get { return this.pageLimit; }
      set {
        Contract.Requires<ArgumentException>(value >= 0);
        this.pageLimit = value;
      }
    }
    #endregion


    #region [Method: Constructor]
    public PaginationUtil() {
      this.includeHeader = true;
      this.headerFormat = "Page {0} of {1}";
      this.headerTextColor = Color.Yellow;
      this.nothingToDisplayString = null;
      this.lineFormatter = null;
      this.lineTextColor = Color.White;
      this.maxLinesPerPage = maxLinesPerPage;
      this.pageLimit = 0;
    }
    #endregion

    #region [Method: Static BuildLinesFromTerms]
    public static List<string> BuildLinesFromTerms(
      IEnumerable terms, Func<object,string> termFormatter = null, string separator = ", ", int maxCharsPerLine = 80
    ) {
      List<string> lines = new List<string>();
      StringBuilder lineBuilder = new StringBuilder();
      foreach (object term in terms) {
        if (term == null && termFormatter == null)
          continue;

        string termString;
        if (termFormatter != null) {
          try {
            termString = termFormatter(term);

            if (termString == null)
              continue;
          } catch (Exception ex) {
            throw new ArgumentException(
              "The method represented by termFormatter has thrown an exception. See inner exception for details.", ex
            );
          }
        } else {
          termString = term.ToString();
        }

        bool goesOnNextLine = (lineBuilder.Length + termString.Length > maxCharsPerLine);
        if (!goesOnNextLine) {
          if (lineBuilder.Length > 0)
            lineBuilder.Append(separator);
          lineBuilder.Append(termString);
        } else {
          // A separator should always be at the end of a line as we know it is followed by another line.
          lineBuilder.Append(separator);
          lines.Add(lineBuilder.ToString());
          lineBuilder.Clear();

          lineBuilder.Append(termString);
        }
      }
      if (lineBuilder.Length > 0)
        lines.Add(lineBuilder.ToString());

      return lines;
    }
    #endregion

    #region [Method: SendPage]
    public void SendPage(TSPlayer player, IEnumerable contents, int pageNumber, int contentCount) {
      if (this.MaxLinesPerPage == 0)
        throw new InvalidOperationException("This PaginationUtil object is invalid, use the non default constructor instead.");

      if (contentCount == 0) {
        if (this.NothingToDisplayString != null)
          player.SendMessage(this.NothingToDisplayString, this.HeaderTextColor);

        return;
      }

      int pageCount = ((contentCount - 1) / this.MaxLinesPerPage) + 1;
      if (this.PageLimit > 0 && pageCount > this.PageLimit)
        pageCount = this.PageLimit;
      if (pageNumber > pageCount)
        pageNumber = pageCount;

      if (this.IncludeHeader)
        player.SendMessage(string.Format(this.HeaderFormat, pageNumber, pageCount), this.HeaderTextColor);

      int listOffset = (pageNumber - 1) * this.MaxLinesPerPage;
      int offsetCounter = 0;
      int lineCounter = 0;
      foreach (object lineContent in contents) {
        if (lineContent == null)
          continue;
        if (offsetCounter++ < listOffset)
          continue;
        if (lineCounter++ == this.MaxLinesPerPage)
          break;

        string lineMessage;
        if (this.LineFormatter != null) {
          try {
            lineMessage = this.LineFormatter(lineContent);
          } catch (Exception ex) {
            throw new InvalidOperationException(
              "The method represented by LineFormatter has thrown an exception. See inner exception for details.", ex
            );
          }
        } else {
          lineMessage = lineContent.ToString();
        }

        if (lineMessage != null)
          player.SendMessage(lineMessage, this.LineTextColor);
      }

      if (lineCounter == 0 && this.NothingToDisplayString != null)
        player.SendMessage(this.NothingToDisplayString, this.HeaderTextColor);
    }

    public void SendPage(TSPlayer player, IList contents, int pageNumber) {
      this.SendPage(player, contents, pageNumber, contents.Count);
    }
    #endregion
  }
}
