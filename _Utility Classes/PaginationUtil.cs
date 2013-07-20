using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public static class PaginationUtil {
    public delegate Tuple<string,Color> LineFormatterDelegate(object lineData, int lineIndex, int pageNumber);

    #region [Nested: Settings Class]
    public class Settings {
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

      #region [Property: IncludeFooter]
      private bool includeFooter;

      public bool IncludeFooter {
        get { return this.includeFooter; }
        set { this.includeFooter = value; }
      }
      #endregion

      #region [Property: FooterFormat]
      private string footerFormat;

      public string FooterFormat {
        get { return this.footerFormat; }
        set {
          Contract.Requires<ArgumentNullException>(value != null);
          this.footerFormat = value;
        }
      }
      #endregion

      #region [Property: FooterTextColor]
      private Color footerTextColor;

      public Color FooterTextColor {
        get { return this.footerTextColor; }
        set { this.footerTextColor = value; }
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
      private LineFormatterDelegate lineFormatter;

      public LineFormatterDelegate LineFormatter {
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
      public Settings() {
        this.includeHeader = true;
        this.headerFormat = "Page {0} of {1}";
        this.includeFooter = false;
        this.footerFormat = "Type /<command> {0} for more.";
        this.headerTextColor = Color.Yellow;
        this.nothingToDisplayString = null;
        this.lineFormatter = null;
        this.lineTextColor = Color.White;
        this.maxLinesPerPage = 4;
        this.pageLimit = 0;
      }
      #endregion
    }
    #endregion

    #region [Method: SendPage]
    public static void SendPage(
      TSPlayer player, int pageNumber, IEnumerable dataToPaginate, int dataToPaginateCount, Settings settings = null
    ) {
      if (settings == null)
        settings = new Settings();

      if (dataToPaginateCount == 0) {
        if (settings.NothingToDisplayString != null)
          player.SendMessage(settings.NothingToDisplayString, settings.HeaderTextColor);

        return;
      }

      int pageCount = ((dataToPaginateCount - 1) / settings.MaxLinesPerPage) + 1;
      if (settings.PageLimit > 0 && pageCount > settings.PageLimit)
        pageCount = settings.PageLimit;
      if (pageNumber > pageCount)
        pageNumber = pageCount;

      if (settings.IncludeHeader)
        player.SendMessage(string.Format(settings.HeaderFormat, pageNumber, pageCount), settings.HeaderTextColor);

      int listOffset = (pageNumber - 1) * settings.MaxLinesPerPage;
      int offsetCounter = 0;
      int lineCounter = 0;
      foreach (object lineData in dataToPaginate) {
        if (lineData == null)
          continue;
        if (offsetCounter++ < listOffset)
          continue;
        if (lineCounter++ == settings.MaxLinesPerPage)
          break;

        string lineMessage;
        Color lineColor = settings.LineTextColor;
        if (lineData is Tuple<string,Color>) {
          var lineFormat = (Tuple<string,Color>)lineData;
          lineMessage = lineFormat.Item1;
          lineColor = lineFormat.Item2;
        } else if (settings.LineFormatter != null) {
          try {
            Tuple<string,Color> lineFormat = settings.LineFormatter(lineData, offsetCounter, pageNumber);
            if (lineFormat == null)
              continue;

            lineMessage = lineFormat.Item1;
            lineColor = lineFormat.Item2;
          } catch (Exception ex) {
            throw new InvalidOperationException(
              "The method referenced by LineFormatter has thrown an exception. See inner exception for details.", ex
            );
          }
        } else {
          lineMessage = lineData.ToString();
        }

        if (lineMessage != null)
          player.SendMessage(lineMessage, lineColor);
      }

      if (lineCounter == 0) {
        if (settings.NothingToDisplayString != null)
          player.SendMessage(settings.NothingToDisplayString, settings.HeaderTextColor);
      } else if (settings.IncludeFooter && pageNumber + 1 <= pageCount) {
        player.SendMessage(string.Format(settings.FooterFormat, pageNumber + 1, pageNumber, pageCount), settings.FooterTextColor);
      }
    }

    public static void SendPage(TSPlayer player, int pageNumber, IList dataToPaginate, Settings settings = null) {
      PaginationUtil.SendPage(player, pageNumber, dataToPaginate, dataToPaginate.Count, settings);
    }
    #endregion

    #region [Method: BuildLinesFromTerms]
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

    #region [Method: TryParsePageNumber]
    public static bool TryParsePageNumber(
      List<string> commandParameters, int expectedParamterIndex, TSPlayer errorMessageReceiver, out int pageNumber
    ) {
      pageNumber = 1;
      if (commandParameters.Count <= expectedParamterIndex)
        return true;

      string pageNumberRaw = commandParameters[expectedParamterIndex];
      if (!int.TryParse(pageNumberRaw, out pageNumber) || pageNumber < 1) {
        if (errorMessageReceiver != null)
          errorMessageReceiver.SendErrorMessage(string.Format("\"{0}\" is not a valid page number.", pageNumberRaw));

        pageNumber = 1;
        return false;
      }

      return true;
    }
    #endregion
  }
}
