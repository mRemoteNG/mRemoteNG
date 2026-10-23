using System.Collections.Generic;
using System.Text;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Csv
{
    /// <summary>
    /// The CSV dialect of the mRemoteNG connection export: RFC 4180 with ';' as the separator.
    /// A field that holds a ';', a double quote or a line break is enclosed in double quotes,
    /// with a double quote inside it written twice.
    /// </summary>
    internal static class SemicolonSeparatedValues
    {
        private static readonly char[] CharactersThatNeedQuotes = [';', '"', '\r', '\n'];

        public static string Quote(string value)
        {
            if (value.IndexOfAny(CharactersThatNeedQuotes) < 0)
                return value;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        /// <summary>
        /// Splits the text into records of fields. Empty lines are skipped.
        /// </summary>
        public static List<string[]> Parse(string text)
        {
            List<string[]> records = [];
            List<string> fields = [];
            StringBuilder field = new();
            int index = 0;

            while (index < text.Length)
            {
                char current = text[index];
                if (current == '"' && field.Length == 0)
                {
                    index = ReadQuotedField(text, index + 1, field);
                }
                else if (current == ';')
                {
                    fields.Add(field.ToString());
                    field.Clear();
                    index++;
                }
                else if (current == '\r' || current == '\n')
                {
                    EndRecord(records, fields, field);
                    index += current == '\r' && index + 1 < text.Length && text[index + 1] == '\n' ? 2 : 1;
                }
                else
                {
                    field.Append(current);
                    index++;
                }
            }

            EndRecord(records, fields, field);
            return records;
        }

        // Reads a quoted field from just after its opening quote and returns the index
        // after its closing quote.
        private static int ReadQuotedField(string text, int index, StringBuilder field)
        {
            while (index < text.Length)
            {
                if (text[index] != '"')
                {
                    field.Append(text[index]);
                    index++;
                }
                else if (index + 1 < text.Length && text[index + 1] == '"')
                {
                    field.Append('"');
                    index += 2;
                }
                else
                {
                    return index + 1;
                }
            }

            return index;
        }

        private static void EndRecord(List<string[]> records, List<string> fields, StringBuilder field)
        {
            if (fields.Count == 0 && field.Length == 0)
                return;

            fields.Add(field.ToString());
            records.Add(fields.ToArray());
            fields.Clear();
            field.Clear();
        }
    }
}
