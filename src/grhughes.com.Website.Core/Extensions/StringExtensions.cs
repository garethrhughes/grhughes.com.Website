namespace grhughes.com.Website.Core.Extensions
{
  using System;
  using System.Globalization;
  using System.Text;
  using System.Text.RegularExpressions;

  public static class StringExtensions
  {
    private const int TARGET_SHORT_ABSTRACT_LENGTH = 100;
    private const int TARGET_LONG_ABSTRACT_LENGTH = 300;
    private static readonly Regex PunctuationPattern = new Regex(@"\W+", RegexOptions.Compiled);
    private static readonly Regex WhitespacePattern = new Regex(@"\s+", RegexOptions.Compiled);

    public static string Slug(this string text)
    {
      return RemoveDiacritics(Trim(RemoveWhitespace(RemovePunctuation(text)))).ToLowerInvariant();
    }

    public static string LimitString(this string text, int limit)
    {
      return text.Length <= limit ? text : text.Substring(0, limit);
    }

    private static string RemoveWhitespace(string text)
    {
      return WhitespacePattern.Replace(text, "-");
    }

    private static string RemovePunctuation(string text)
    {
      return PunctuationPattern.Replace(text, " ");
    }

    private static string Trim(string text)
    {
      return text.Trim('-');
    }

    // Taken from http://blogs.msdn.com/michkap/archive/2007/05/14/2629747.aspx
    private static string RemoveDiacritics(string text)
    {
      string normalized = text.Normalize(NormalizationForm.FormD);
      var builder = new StringBuilder();

      foreach (var t in normalized)
      {
        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(t);
        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        {
          builder.Append(t);
        }
      }
      return (builder.ToString().Normalize(NormalizationForm.FormC));
    }

    public static string ShortAbstract(this string longText)
    {
      return Abstract(longText, TARGET_SHORT_ABSTRACT_LENGTH);
    }

    public static string LongAbstract(this string longText)
    {
      return Abstract(longText, TARGET_LONG_ABSTRACT_LENGTH);
    }

    private static string Abstract(string longText, int desiredLength)
    {
      if (longText.Length <= desiredLength)
        return longText;

      int truncatePoint = NearestWhitespace(longText, desiredLength);
      if (truncatePoint == -1)
        truncatePoint = desiredLength;

      return longText.Remove(truncatePoint - 1) + '…';
    }

    private static int NearestWhitespace(string text, int targetLength)
    {
      const int nearestAcceptableDistance = 10;
      int startLookingAt = targetLength - nearestAcceptableDistance;
      int stopLookingAt = Math.Min(text.Length, targetLength + nearestAcceptableDistance);
      int nearestWhitespace = -1;
      for (int index = startLookingAt; index < stopLookingAt; index++)
      {
        bool isWhiteSpace = char.IsWhiteSpace(text[index]);
        bool isNearer = Math.Abs(targetLength - nearestWhitespace) > Math.Abs(targetLength - index);
        if (isWhiteSpace && isNearer)
        {
          nearestWhitespace = index;
        }
      }
      return nearestWhitespace;
    }

    public static string AddOrdinal(this string text, int number)
    {
      if (number > 1)
        text = string.Format("{0}s", text);

      return text;
    }

    public static string ConvertNewLinesToParagraphs(this string str)
    {
      if (str == null)
        return null;

      string[] chunks = str.Split('\n');
      var builder = new StringBuilder();

      foreach (var chunk in chunks)
      {
        if (chunk.Trim().Length != 0)
          builder.Append(string.Format("<p>{0}</p>", chunk.Trim()));
      }

      return builder.ToString();
    }

    public static string ConvertUrlsToLinks(this string msg)
    {
      const string regex =
        @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
      var r = new Regex(regex, RegexOptions.IgnoreCase);
      return r.Replace(msg, "<a href=\"$1\">$1</a>").Replace("href=\"www", "href=\"http://www");
    }

    public static string SocialDate(this DateTime d)
    {
      // 1.
      // Get time span elapsed since the date.
      TimeSpan s = DateTime.Now.Subtract(d);

      // 2.
      // Get total number of days elapsed.
      var dayDiff = (int) s.TotalDays;

      // 3.
      // Get total number of seconds elapsed.
      var secDiff = (int) s.TotalSeconds;

      // 4.
      // Don't allow out of range values.
      if (dayDiff < 0 || dayDiff >= 31)
      {
        return null;
      }

      // 5.
      // Handle same-day times.
      if (dayDiff == 0)
      {
        // A.
        // Less than one minute ago.
        if (secDiff < 60)
        {
          return "just now";
        }
        // B.
        // Less than 2 minutes ago.
        if (secDiff < 120)
        {
          return "1 minute ago";
        }
        // C.
        // Less than one hour ago.
        if (secDiff < 3600)
        {
          return string.Format("{0} minutes ago",
                               Math.Floor((double) secDiff/60));
        }
        // D.
        // Less than 2 hours ago.
        if (secDiff < 7200)
        {
          return "1 hour ago";
        }
        // E.
        // Less than one day ago.
        if (secDiff < 86400)
        {
          return string.Format("{0} hours ago",
                               Math.Floor((double) secDiff/3600));
        }
      }
      // 6.
      // Handle previous days.
      if (dayDiff == 1)
      {
        return "yesterday";
      }
      if (dayDiff < 7)
      {
        return string.Format("{0} days ago",
                             dayDiff);
      }
      if (dayDiff < 31)
      {
        return string.Format("{0} weeks ago",
                             Math.Ceiling((double) dayDiff/7));
      }
      return null;
    }
  }
}