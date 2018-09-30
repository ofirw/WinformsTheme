namespace WinformsTheme.Parsers
{
    using Pidgin;
    using System.Collections.Generic;
    using System.Linq;
    using WinformsTheme.Elements;
    using static Pidgin.Parser;
    using static Pidgin.Parser<char>;

    public static class PidginWinformThemeParsers
    {
        private static readonly Parser<char, char> LetterDigitOrUnderScore = LetterOrDigit.Or(Char('_'));
        private static readonly Parser<char, char> Colon = Char(':').Between(SkipWhitespaces);
        private static readonly Parser<char, char> SemiColon = Char(';').Between(SkipWhitespaces);
        private static readonly Parser<char, char> BeginBlock = Char('{').Between(SkipWhitespaces);
        private static readonly Parser<char, char> EndBlock = Char('}').Between(SkipWhitespaces);
        private static readonly Parser<char, string> FileName = OneOf(Char('.'), Char('_'), Char('-'), LetterOrDigit).ManyString().Before(SemiColon);

        private static Parser<char, IEnumerable<T>> BlockOf<T>(Parser<char, T> parser) =>
            parser.Between(SkipWhitespaces).Many().Between(BeginBlock, EndBlock);

        private static Parser<char , SelectorBlock> SelectorBlockOf(Parser<char,string> parser) =>
            parser.Then(Identifier, (c,i)=> c+i)
            .Then(BlockOf(AttributeOrSelector.Between(SkipWhitespaces)), (name, block) => new SelectorBlock(name, block))
            .Between(SkipWhitespaces);

        public static readonly Parser<char, string> Identifier = Letter.Then(LetterDigitOrUnderScore.ManyString(), (a, b) => a + b).Between(SkipWhitespaces);

        public static readonly Parser<char, string> StringValue =
            LetterDigitOrUnderScore.Or(Char('@')).Or(Char('#'))
            .Then(LetterDigitOrUnderScore.Or(Whitespace).Or(Char(',')).ManyString(), (a, b) => a + b)
            .Between(SkipWhitespaces).Select(s => s.Trim());

        public static readonly Parser<char, Property> Attribute =
            Identifier.Before(Colon)
            .Then(StringValue, (name, val) => new Property(name, val))
            .Before(SemiColon);

        public static readonly Parser<char, VariableBlock> VariableBlock =
            String("$Variables").Then(BlockOf(Attribute)).Select(v => new VariableBlock(v)).Between(SkipWhitespaces);

        public static readonly Parser<char, IncludeBlock> IncludeBlock =
            String("$Include").Then(BlockOf(FileName)).Select(v => new IncludeBlock(v.Select(s=>new StringValue(s)))).Between(SkipWhitespaces);

        private static readonly Parser<char, IWinformThemeElement> AttributeOrSelector = Attribute.OfType<IWinformThemeElement>().Or(Rec(() => AllSelectors.OfType<IWinformThemeElement>()));

        public static readonly Parser<char, SelectorBlock> SelectorByType = SelectorBlockOf(FromResult(""));
        public static readonly Parser<char, SelectorBlock> SelectorById = SelectorBlockOf(String("#"));
        public static readonly Parser<char, SelectorBlock> SelectorByClass = SelectorBlockOf(String("."));
        public static readonly Parser<char, SelectorBlock> AllSelectors = SelectorByClass.Or(SelectorById).Or(SelectorByType);

        public static readonly Parser<char, IEnumerable<IWinformThemeElement>> ThemeParser =
            Try(VariableBlock.OfType<IWinformThemeElement>())
            .Or(Try(IncludeBlock.OfType<IWinformThemeElement>())
                .Or(AllSelectors.OfType<IWinformThemeElement>()))
            .Many()
            .Between(SkipWhitespaces)
            .Before(End());
    }
}
