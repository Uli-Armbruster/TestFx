using System;
using System.Globalization;
using System.Text;

namespace TestFx.Console
{
  /// <summary>
  /// Writes specially formatted service messages for TeamCity.
  /// These messages are interpreted by TeamCity to perform some task.
  /// See also: http://www.jetbrains.net/confluence/display/TCD3/Build+Script+Interaction+with+TeamCity
  /// </summary>
  public class TeamCityServiceMessageWriter
  {
    private readonly Action<string> writer;

    public TeamCityServiceMessageWriter (Action<string> writer)
    {
      if (writer == null)
        throw new ArgumentNullException("writer");

      this.writer = writer;
    }

    public void WriteProgressMessage (string message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      WriteMessage(
          builder =>
          {
            builder.Append("progressMessage '");
            AppendEscapedString(builder, message);
            builder.Append('\'');
          });
    }

    public void WriteProgressStart (string message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      WriteMessage(
          builder =>
          {
            builder.Append("progressStart '");
            AppendEscapedString(builder, message);
            builder.Append('\'');
          });
    }

    public void WriteProgressFinish (string message)
    {
      if (message == null)
        throw new ArgumentNullException("message");

      WriteMessage(
          builder =>
          {
            builder.Append("progressFinish '");
            AppendEscapedString(builder, message);
            builder.Append('\'');
          });
    }

    public void WriteTestSuiteStarted (string name)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      WriteMessage(
          builder =>
          {
            builder.Append("testSuiteStarted name='");
            AppendEscapedString(builder, name);
            builder.Append('\'');
          });
    }

    public void WriteTestSuiteFinished (string name)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      WriteMessage(
          builder =>
          {
            builder.Append("testSuiteFinished name='");
            AppendEscapedString(builder, name);
            builder.Append('\'');
          });
    }

    public void WriteTestStarted (string name, bool captureStandardOutput)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      WriteMessage(
          builder =>
          {
            builder.Append("testStarted name='");
            AppendEscapedString(builder, name);
            builder.Append("' captureStandardOutput='");
            AppendEscapedString(builder, captureStandardOutput ? @"true" : @"false");
            builder.Append('\'');
          });
    }

    public void WriteTestFinished (string name, TimeSpan duration)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      WriteMessage(
          builder =>
          {
            builder.Append("testFinished name='");
            AppendEscapedString(builder, name);
            builder.Append("' duration='");
            builder.Append(((int) duration.TotalMilliseconds).ToString(CultureInfo.InvariantCulture));
            builder.Append('\'');
          });
    }

    public void WriteTestIgnored (string name, string message)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (message == null)
        throw new ArgumentNullException("message");

      WriteMessage(
          builder =>
          {
            builder.Append("testIgnored name='");
            AppendEscapedString(builder, name);
            builder.Append("' message='");
            AppendEscapedString(builder, message);
            builder.Append('\'');
          });
    }

    public void WriteTestStdOut (string name, string text)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (text == null)
        throw new ArgumentNullException("text");

      WriteMessage(
          builder =>
          {
            builder.Append("testStdOut name='");
            AppendEscapedString(builder, name);
            builder.Append("' out='");
            AppendEscapedString(builder, text);
            builder.Append('\'');
          });
    }

    public void WriteTestStdErr (string name, string text)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (text == null)
        throw new ArgumentNullException("text");

      WriteMessage(
          builder =>
          {
            builder.Append("testStdErr name='");
            AppendEscapedString(builder, name);
            builder.Append("' out='");
            AppendEscapedString(builder, text);
            builder.Append('\'');
          });
    }

    public void WriteTestFailed (string name, string message, string details)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (message == null)
        throw new ArgumentNullException("message");
      if (details == null)
        throw new ArgumentNullException("details");

      WriteMessage(
          builder =>
          {
            builder.Append("testFailed name='");
            AppendEscapedString(builder, name);
            builder.Append("' message='");
            AppendEscapedString(builder, message);
            builder.Append("' details='");
            AppendEscapedString(builder, details);
            builder.Append('\'');
          });
    }

    public void WriteTestFailedWithComparisonFailure (string name, string message, string details, string expected, string actual)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      if (message == null)
        throw new ArgumentNullException("message");
      if (details == null)
        throw new ArgumentNullException("details");
      if (expected == null)
        throw new ArgumentNullException("expected");
      if (actual == null)
        throw new ArgumentNullException("actual");

      WriteMessage(
          builder =>
          {
            builder.Append("testFailed name='");
            AppendEscapedString(builder, name);
            builder.Append("' type='comparisonFailure' message='");
            AppendEscapedString(builder, message);
            builder.Append("' details='");
            AppendEscapedString(builder, details);
            builder.Append("' expected='");
            AppendEscapedString(builder, expected);
            builder.Append("' actual='");
            AppendEscapedString(builder, actual);
            builder.Append('\'');
          });
    }

    public void WriteError (string message, string details)
    {
      WriteMessage(
          builder =>
          {
            builder.Append("message test='");
            AppendEscapedString(builder, message);
            builder.Append("' errorDetails='");
            AppendEscapedString(builder, details);
            builder.Append("' status='ERROR'");
          });
    }

    private void WriteMessage (Action<StringBuilder> formatter)
    {
      var builder = new StringBuilder();
      builder.Append("##teamcity[");
      formatter(builder);
      builder.Append(']');
      writer(builder.ToString());
    }

    private static void AppendEscapedString (StringBuilder builder, string rawString)
    {
      if (rawString == null)
        return;
      foreach (var c in rawString)
      {
        switch (c)
        {
          case '\n':
            builder.Append("|n");
            break;
          case '\'':
            builder.Append("|'");
            break;
          case '\r':
            builder.Append("|r");
            break;
          case '|':
            builder.Append("||");
            break;
          case ']':
            builder.Append("|]");
            break;
          case '\u0085': // \u0085 (next line) => |x
            builder.Append("|x");
            break;
          case '\u2028': // \u2028 (line separator) => |l
            builder.Append("|l");
            break;
          case '\u2029':
            builder.Append("|p");
            break;
          default:
            builder.Append(c);
            break;
        }
      }
    }
  }
}