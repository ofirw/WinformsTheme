using System.IO;
using System.Linq;
using WinformsTheme;
using Xunit;

namespace WinformsThemeTests
{
    public class WinformsThemeLoaderTest
    {
        [Fact]
        public void LoadSimpleTheme()
        {
            File.WriteAllText("test.theme", "$Variables {v1:1;v2:2;}.Button { BackColor:Blue;}");

            var theme = WinformsThemeLoader.Load("test.theme");

            Assert.Equal(1, theme.Selectors.Count());
            Assert.Equal(2, theme.Variables.ElementsDictionary.Count);
        }

        [Fact]
        public void LoadThemeWithComments()
        {
            File.WriteAllText("test.theme",
                @"// SomeComments 
                       $Variables {v1:1;v2:2;} // another
                       .Button { BackColor:Blue;
                            /* A block
                            *
                            */
                            }
                 ");

            var theme = WinformsThemeLoader.Load("test.theme");

            Assert.Equal(1, theme.Selectors.Count());
            Assert.Equal(2, theme.Variables.ElementsDictionary.Count);
        }

        [Fact]
        public void LoadThemeWithInclude()
        {
            File.WriteAllText("test.theme", "$Variables {v1:1;v2:2;}.Button { BackColor:Blue;}");
            File.WriteAllText("withInclude.theme", "$Include {test.theme;}");

            var theme = WinformsThemeLoader.Load("withInclude.theme");

            Assert.Equal(0, theme.Selectors.Count());
            Assert.Equal(0, theme.Variables.ElementsDictionary.Count);
            Assert.Equal(1, theme.IncludeThemes.Count);
        }
    }
}
