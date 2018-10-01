using System;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinformsTheme;
using WinformsTheme.Elements;
using WinformsTheme.Parsers;

namespace WinformsLessCssTests
{
    public class PidginTest
    {
        private static void AssertParsedStringIsExpected(Parser<char, string> parser, string expected, string input)
        {
            Assert.Equal(expected, parser.ParseOrThrow(input));
        }

        private static void AssertEqualAttribute(string expectedIdentifier, string expectedValue, Property attribute)
        {
            Assert.Equal(expectedIdentifier, attribute.Identifier);
            Assert.Equal(expectedValue, attribute.GetRawValue());
        }

        private static void AssertEqualAttribute(KeyValuePair<string, string> expectedProperty, Property attribute) =>
            AssertEqualAttribute(expectedProperty.Key.Trim(), expectedProperty.Value.Trim(), attribute);

        private static void AssertBlockWithDummyProp<Y>(Parser<char, WinformThemeBlock<Y>> parser, string expectedId, bool checkNested=false) where Y : IWinformThemeElement
        {
            var expectedPropeties = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>("BackColor", "Black" ),
                new KeyValuePair<string,string>("ForeColor", "White"),
                new KeyValuePair<string,string>("  SomeProp  ", "  Black  " )
            };

            var propString = string.Join("\n", expectedPropeties.Select(kvp => $"{kvp.Key} : {kvp.Value};"));
            StringBuilder sb = new StringBuilder(expectedId).Append(" { ").Append(propString).Append(" ");
            string nestedExpectedId = ".Nested" + expectedId;
            var nestedExpectedProperties = expectedPropeties.Select(kvp => new KeyValuePair<string,string>("Nest" + kvp.Key.Trim(), "Nest" + kvp.Value.Trim())).ToList();

            if (checkNested)
            {
                propString = string.Join("\n", nestedExpectedProperties.Select(kvp => $"{kvp.Key} : {kvp.Value};"));
                sb.Append(nestedExpectedId).Append(" { ").Append(propString).Append(" } ");
            }
            sb.Append(" }");

            var selector = parser.ParseOrThrow(sb.ToString());

            AssertSelector(expectedId, expectedPropeties, selector);

            if (checkNested)
            {
                var nestedSelector = selector.ElementsDictionary.Values.OfType<SelectorBlock>().Cast<WinformThemeBlock<IWinformThemeElement>>().Single();
                AssertSelector(nestedExpectedId, nestedExpectedProperties, nestedSelector);
            }
        }

        private static void AssertSelector<Y>(string expectedId, List<KeyValuePair<string, string>> expectedPropeties, WinformThemeBlock<Y> selector) where Y : IWinformThemeElement
        {
            Assert.Equal(expectedId, selector.Identifier);
            var elements = selector.ElementsDictionary.Values.OfType<Property>().ToList();
            for (int i = 0; i < expectedPropeties.Count; i++)
            {
                AssertEqualAttribute(expectedPropeties[i], elements[i]);
            }
        }

        [Fact]
        public void IdentifierParser()
        {
            var parser = PidginWinformThemeParsers.Identifier;
            AssertParsedStringIsExpected(parser, "ABC123", "ABC123");
            AssertParsedStringIsExpected(parser, "ABC_123", " ABC_123 ");
            Assert.Throws<ParseException>(() => parser.ParseOrThrow("123ABC"));
        }

        [Fact]
        public void StringValueParser()
        {
            var parser = PidginWinformThemeParsers.StringValue;
            AssertParsedStringIsExpected(parser, "ABC123", "ABC123");
            AssertParsedStringIsExpected(parser, "ABC_123", " ABC_123 ");
            AssertParsedStringIsExpected(parser, "123ABC", " 123ABC ");
            AssertParsedStringIsExpected(parser, "@ABC123", "@ABC123");
            AssertParsedStringIsExpected(parser, "(*&^%$ TERTRE", "(*&^%$ TERTRE");
            AssertParsedStringIsExpected(parser, "123", "123;456");
            AssertParsedStringIsExpected(parser, "123;456", "\"123;456\"");
        }

        [Fact]
        public void AttributeParser()
        {
            var parser = PidginWinformThemeParsers.Attribute;
            AssertEqualAttribute("Key", "Value", parser.ParseOrThrow("Key : Value;"));
            AssertEqualAttribute("Key", "Value", parser.ParseOrThrow("  Key : Value  ;  "));
        }

        [Fact]
        public void VariableBlockParse()
        {
            AssertBlockWithDummyProp(PidginWinformThemeParsers.VariableBlock.OfType<WinformThemeBlock<Property>>(), "$Variables");
        }

        [Fact]
        public void IncludeBlockParser()
        {
            var parser = PidginWinformThemeParsers.IncludeBlock;
            var filesNames1 = parser.ParseOrThrow("$Include { file1.ext; file2; file_3.theme;}").ElementsDictionary;
            Assert.Equal("file1.ext", filesNames1["file1.ext"].Identifier);
            Assert.Equal("file2", filesNames1["file2"].Identifier);
            Assert.Equal("file_3.theme", filesNames1["file_3.theme"].Identifier);

            var filesNames2 = parser.ParseOrThrow("  $Include { file1.ext   ;  file2   ; file_3.theme  ;   }   ").ElementsDictionary;
            Assert.Equal("file1.ext", filesNames2["file1.ext"].Identifier);
            Assert.Equal("file2", filesNames2["file2"].Identifier);
            Assert.Equal("file_3.theme", filesNames2["file_3.theme"].Identifier);
        }

        [Fact]
        public void SelectorByTypeParser() => AssertBlockWithDummyProp(PidginWinformThemeParsers.SelectorByType.OfType<WinformThemeBlock<IWinformThemeElement>>(), "Form");

        [Fact]
        public void SelectorByIdParser() => AssertBlockWithDummyProp(PidginWinformThemeParsers.SelectorById.OfType<WinformThemeBlock<IWinformThemeElement>>(), "#Form");

        [Fact]
        public void SelectorByClassParser() => AssertBlockWithDummyProp(PidginWinformThemeParsers.SelectorByClass.OfType<WinformThemeBlock<IWinformThemeElement>>(), ".Form");

        [Fact]
        public void AllSelectorsParser()
        {
            AssertBlockWithDummyProp(PidginWinformThemeParsers.AllSelectors.OfType<WinformThemeBlock<IWinformThemeElement>>(), "Form");
            AssertBlockWithDummyProp(PidginWinformThemeParsers.AllSelectors.OfType<WinformThemeBlock<IWinformThemeElement>>(), "#Form");
            AssertBlockWithDummyProp(PidginWinformThemeParsers.AllSelectors.OfType<WinformThemeBlock<IWinformThemeElement>>(), ".Form");
        }

        [Fact]
        public void NestedSelectors()
        {
            var parser = PidginWinformThemeParsers.AllSelectors;
            AssertBlockWithDummyProp(PidginWinformThemeParsers.AllSelectors.OfType<WinformThemeBlock<IWinformThemeElement>>(), "Form", true);
        }

        [Fact]
        public void CompleteParsing()
        {
            PidginWinformThemeParsers.ThemeParser.ParseOrThrow(Final);
        }

        private const string Final = @"
$Variables {
	Color:Blue;
	Meir:Banai;
}

$Include {
	default.theme;
	Pik.Pak;
}

#Form {
	BackColor: Blue;
	#Button {
		BackColor: Red;
		.Ok{
			Enabled: false;
		}
	}
}

Button 
{
	BackColor : Red;

	.BTN { BackColor: Yellow; }
	#btn2 { BackColor: #676767;}
}

#panel1 {
	BackColor: Orange;
}

.Ok
{
	ForeColor: White;
}";
    }
}
