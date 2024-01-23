using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using NewtonsoftSerializer = Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace Tx.Core.Extensions.String
{

    public static class StringEx
    {
        private static readonly NewtonsoftSerializer.JsonSerializerSettings JsonSerializerSettings = new NewtonsoftSerializer.JsonSerializerSettings
        {
            ReferenceLoopHandling = NewtonsoftSerializer.ReferenceLoopHandling.Ignore,
            StringEscapeHandling = NewtonsoftSerializer.StringEscapeHandling.EscapeHtml,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static bool IsNumeric(this string str) => str.All(char.IsDigit);

        public static string ToHex(this string str) => !str.IsNumeric() ? null : $"{str:X8}";

        public static string Left(this string str, int len) => str.Substring(0, Math.Min(len, str.Length));

        public static string Right(this string str, int len) => str.Substring(str.Length - len, len);

        public static string Truncate(this string str, int len) => str.Length > len ? str.Substring(0, (int)(str.Length - len)) : str;

        public static string TruncFromRight(this string str, int len) => str.Substring(0, str.Length - len);

        public static string TruncFromLeft(this string str, int len) => str.Substring(len, str.Length - len);

        public static string CopyUntil(this string str, int start, int len) => str.Substring(start, len);

        public static decimal ToDecimal(this string str, int decimales) => decimal.Round(Convert.ToDecimal(str), decimales);

        public static double ToDouble(this string input, double defaultValue = default) => double.TryParse(input, out var value) ? value : defaultValue;
        public static long ToLong(this string input, long defaultValue = default) => long.TryParse(input, out var value) ? value : defaultValue;

        public static bool ToBoolean(this string str)
        {
            bool result;
            try
            {
                result = Convert.ToBoolean(str);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public static bool IsTrue(this string input) => input != "false" && input != "0";

        public static byte[] ToUtf8Bytes(this object value, JsonSerializerOptions options = null)
        {
            if (value == null) { return default; }
            try
            {
                return options != null ? JsonSerializer.SerializeToUtf8Bytes(value, options) : JsonSerializer.SerializeToUtf8Bytes(value);
            }
            catch { return default; }
        }

        public static string ToJson(this object value)
        {
            if (value == null) { return null; }
            try { return NewtonsoftSerializer.JsonConvert.SerializeObject(value); }
            catch { return null; }
        }

        // using Newtonsoft
        public static string ToJson(this object value, NewtonsoftSerializer.JsonSerializerSettings settings)
        {
            if (value == null) { return null; }

            NewtonsoftSerializer.JsonSerializerSettings opSettings = settings ?? JsonSerializerSettings;

            try { return NewtonsoftSerializer.JsonConvert.SerializeObject(value, opSettings); }
            catch { return null; }
        }


        // using Newtonsoft
        public static T ParseTo<T>(this string str)
        {
            if (string.IsNullOrEmpty(str)) { return default(T); }
            try { return NewtonsoftSerializer.JsonConvert.DeserializeObject<T>(str); }
            catch (Exception ex) { Console.WriteLine(ex.Message); return default(T); }
        }

        public static string CopyUntilChar(this string str, int startindex, char caracter) => str.Substring(startindex, str.IndexOf(caracter));

        // Return a substring from x char to the end
        public static string CopyFromChar(this string str, char caracter)
        {
            var from = str.IndexOf(caracter) + 1;
            return str.Substring(from, str.Length - from);
        }

        // Replace a chat at x position
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        public static bool ArraySearch(this ArrayList lista, string value)
        {
            var r = -1;
            if (lista != null) { r = lista.BinarySearch(value); }
            return r > 0;
        }

        public static string Apostrophe<T>(this T data) => $"\'{data}\'";

        /// <summary>
        /// string value = "net";
        /// bool isIn = value.In("dot", "net", "languages"); //true
        /// isIn = value.In("dot", "note", "languages"); //false
        /// </summary>
        public static bool In<T>(this T x, params T[] args) => args.Contains(x);

        public static string Space(int count) => string.Empty.PadLeft(count);

        public static double Val(this string value)
        {
            var result = string.Empty;
            foreach (char c in value)
            {
                if (char.IsNumber(c) || c.Equals('.') && result.Count(x => x.Equals('.')) == 0) { result += c; }
                else if (!c.Equals(' ')) { return string.IsNullOrEmpty(result) ? 0 : Convert.ToDouble(result); }
            }

            return string.IsNullOrEmpty(result) ? 0 : Convert.ToDouble(result);
        }

        public static string EncodeToBase64(this string plainText) => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        public static string DecodeFromBase64(this string base64EncodedData) => Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
        public static string ToBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);
        public static byte[] GetBytes(this string str) => Encoding.UTF8.GetBytes(str);
        public static int WordsCount(this string input) => Regex.Matches(input, @"[^\s]+").Count;
        public static string Replace(this string input, string word, string with, RegexOptions caseOption) => Regex.Replace(input, word, with, caseOption);
        public static int TryParse(this string input, int defaultValue) => int.TryParse(input, out var value) ? value : defaultValue;
        public static int ToInt(this string input, int defaultValue = default) => int.TryParse(input, out var value) ? value : defaultValue;
        public static int CleanAsInt(this string input, int defaultValue)
        {
            var strNumber = string.Concat(input.Where(char.IsDigit));
            return strNumber.ToInt(0);
        }

        public static decimal CleanAsDecimal(this string input, int decimales)
        {
            var strNumber = string.Concat(input.Where(x => char.IsDigit(x) || x == ','));
            return strNumber.ToDecimal(decimales);
        }

        public static string CleanAsString(this string input) => string.Concat(input.Where(char.IsLetter));

        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if ((text.StartsWith("{") && text.EndsWith("}")) || //For object
                (text.StartsWith("[") && text.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch (NewtonsoftSerializer.JsonReaderException ex) { return false; }
                catch (Exception ex) { return false; }
            }

            return false;
        }
    }
}
