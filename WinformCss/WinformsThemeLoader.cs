using Pidgin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WinformsTheme.Elements;
using WinformsTheme.Parsers;

namespace WinformsTheme
{
    public static class WinformsThemeLoader
    {
        public static Theme Load(string fileName)
        {
            var themeText = File.ReadAllText(fileName);

            themeText = RemoveComments(themeText);

            IEnumerable<IWinformThemeElement> elements = PidginWinformThemeParsers.ThemeParser.ParseOrThrow(themeText);

            var includeBlock = elements
                .OfType<IncludeBlock>()
                .DefaultIfEmpty(new IncludeBlock(Enumerable.Empty<StringValue>()))
                .Single();

            var theme = new Theme(elements);

            foreach (var entry in includeBlock.ElementsDictionary.Values)
            {
                var subTheme = Load(entry.Identifier);
                theme.IncludeThemes.Add(subTheme);
            }

            return theme;
        }

        private static string RemoveComments(string themeText)
        {
            const string commentsRegex = @"(\/\/.*)|(\/\*(.|\n)*?\*\/)";
            return Regex.Replace(themeText, commentsRegex, "");
        }
    }
}
