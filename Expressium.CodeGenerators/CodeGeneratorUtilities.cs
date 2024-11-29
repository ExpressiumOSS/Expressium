using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Expressium.CodeGenerators
{
    public static class CodeGeneratorUtilities
    {
        public static bool IsValidURI(string value)
        {
            try
            {
                new Uri(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidClassName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // Validates string starts with a letter followed by any number of letters or digits...
            string pattern = @"^[a-zA-Z]\w*$";

            return Regex.IsMatch(value, pattern);
        }

        public static string CamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.RemoveIllegalCharacters();
            value = value.CapitalizeWords();
            value = value.RemoveWhiteSpaces();

            if (value.Length == 1)
                return value[0].ToString().ToLower();

            return value[0].ToString().ToLower() + value.Substring(1);
        }

        public static string PascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.RemoveIllegalCharacters();
            value = value.CapitalizeWords();
            value = value.RemoveWhiteSpaces();

            return value;
        }

        public static string RemoveIllegalCharacters(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Removes anything that is not a word, apostrophes and underscores...
            value = Regex.Replace(value, @"[^\w\']|[_]", " ");

            // Removes words using apostrophes for abreviations...
            value = Regex.Replace(value, @"[\']", "");

            // Removes all exceeding whitespaces to make a more uniform string...
            return Regex.Replace(value, @"\s{2,}", " ").Trim();
        }

        public static string CapitalizeWords(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(value, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }

        public static string RemoveWhiteSpaces(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Remove all whitespaces spaces, tabs and newlines in string...
            return Regex.Replace(value, @"\s+", "");
        }

        public static string EscapeDoubleQuotes(this string value)
        {
            return value.Replace("\"", "\\\"");
        }

        public static string GenerateRandomString(int length)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
