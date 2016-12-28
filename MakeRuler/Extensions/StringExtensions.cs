using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MakeRuler.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ToLines(this string text)
        {
            return Regex.Split(text, "\r\n|\r|\n").ToList();
        }

        public static List<string> ToWords(this string text)
        {
            return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string[] Split(this string str, string splitter, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new[] { splitter }, options);
        }
    }
}