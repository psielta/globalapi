using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GlobalLib.Strings
{
    public static class UtlStrings
    {
        public static string OnlyInteger(string text)
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }
        public static string RemoveSpecialCharacters(string text, bool allowSpace = false)
        {
            string ret;

            if (allowSpace)
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
            else
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ]+?", string.Empty);

            return ret;
        }
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                    sb.Append(text[i]);
                else
                    sb.Append(s_Diacritics[text[i]]);
            }

            return sb.ToString();
        }

        private static readonly char[] s_Diacritics = GetDiacritics();
        private static char[] GetDiacritics()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++)
                accents[i] = (char)i;

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }

        public static string FormatProductName(string nmProduto)
        {
            if (string.IsNullOrEmpty(nmProduto))
                return nmProduto;

            string formattedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nmProduto.ToLower());
            return formattedName;
        }

        public static string CommaText(List<string> list)
        {
            return string.Join(",", list);
        }

        public static string CommaText<T>(List<T> list)
        {
            return string.Join(",", list.Select(item => item?.ToString()));
        }

        public static string CommaText<T>(List<T> list, char delimiter)
        {
            return string.Join(",", list.Select(item => $"{delimiter}{item?.ToString()}{delimiter}"));
        }

        public static string CommaText(List<string> list, char delimiter)
        {
            return string.Join(",", list.Select(item => $"{delimiter}{item}{delimiter}"));
        }

        public static string QuotedStr(string input, char delimiter = '\'')
        {
            if (input == null)
                return $"{delimiter}{delimiter}";

            var escaped = input.Replace(delimiter.ToString(), new string(delimiter, 2));
            return $"{delimiter}{escaped}{delimiter}";
        }

        // Versão básica: formato dd/MM/yyyy
        public static string DateToStr(DateOnly date)
        {
            return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        // Versão com formato personalizado
        public static string DateToStr(DateOnly date, string format)
        {
            return date.ToString(format, CultureInfo.InvariantCulture);
        }

        // Versão com cultura personalizada
        public static string DateToStr(DateOnly date, CultureInfo culture)
        {
            return date.ToString("d", culture);
        }
    }
}
