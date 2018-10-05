using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinformsTheme;
using WinformsTheme.Elements;
using WinformsTheme.Extensions;
using Xunit;
using Xunit.Sdk;

namespace WinformsThemeTests
{
    public class WinformThemeTest
    {
        private static void ApplyAndAssertBackColorRed(Form form, string identifier, params Control[] assertOnControls)
        {
            foreach (var control in assertOnControls) control.BackColor = Color.Black;
            form.BackColor = Color.Black;

            var theme = new Theme(new[] { new SelectorBlock(identifier, new[] { new Property("BackColor", "Red") }) });
            theme.ApplyTheme(form);

            if (assertOnControls.Length == 0) assertOnControls = new[] { form };
            Assert.All(assertOnControls, control => Assert.Equal(Color.Red, control.BackColor));
        }

        [Fact]
        public void SelectorByType()
        {
            var form = new TestForm();
            form.button1.BackColor = Color.Black;

            ApplyAndAssertBackColorRed(form, "Form");
            Assert.Equal(Color.Black, form.button1.BackColor);
            ApplyAndAssertBackColorRed(form, "TestForm");
            ApplyAndAssertBackColorRed(form, "Button", form.DescendantsControls().OfType<Button>().ToArray());
        }

        [Fact]
        public void SelectorByName()
        {
            var form = new TestForm { Name = "TestName" };
            ApplyAndAssertBackColorRed(form, "#TestName");

            form.button1.Name = "BTN";
            form.button2.BackColor = Color.Black;
            ApplyAndAssertBackColorRed(form, "#BTN", form.button1);
            Assert.Equal(Color.Black, form.button2.BackColor);
        }

        [Fact]
        public void SelectorByClass()
        {
            var form = new TestForm();

            form.Tag = "Red";
            ApplyAndAssertBackColorRed(form, ".Red");

            form.Tag = "SomeClass NotRed";
            Assert.Throws<AllException>(()=> ApplyAndAssertBackColorRed(form, ".Red"));

            form.Tag = "SomeClass Red";
            ApplyAndAssertBackColorRed(form, ".Red");

            form.button1.Tag = "BTN Red";
            form.button2.BackColor = Color.Black;

            ApplyAndAssertBackColorRed(form, ".BTN", form.button1);
            Assert.Equal(Color.Black, form.button2.BackColor);
        }

        [Fact]
        public void SelectWithVariables()
        {
            IWinformThemeElement variables = new VariableBlock(new[] { new Property("colorOfSky", "Blue") });
            IWinformThemeElement selector = new SelectorBlock("Form", new[] { new Property("BackColor", "@colorOfSky") });
            var theme = new Theme(new[] { variables, selector });
            var form = new TestForm { BackColor = Color.Black };

            theme.ApplyTheme(form);

            Assert.Equal(Color.Blue, form.BackColor);
        }

        [Fact]
        public void NestedSelectors()
        {
            var form = new TestForm();
            form.button1.Tag = form.button2.Tag = "yellow";
            form.BackColor = form.button1.BackColor = form.button2.BackColor = Color.Black;

            IWinformThemeElement yellowClassSelector = new SelectorBlock(".yellow", new[] { new Property("BackColor", "Yellow") });
            IWinformThemeElement panel1Selector = new SelectorBlock("#panel1", new[] { new Property("BackColor", "Blue"), yellowClassSelector });
            var theme = new Theme(new[] { panel1Selector });

            theme.ApplyTheme(form);
            Assert.Equal(Color.Black, form.BackColor);
            Assert.Equal(Color.Black, form.button1.BackColor);
            Assert.Equal(Color.Yellow, form.button2.BackColor);
            Assert.Equal(Color.Blue, form.panel1.BackColor);
        }

        [Fact]
        public void HtmlColorTest()
        {
            var form = new TestForm { BackColor = Color.Black };
            const string htmlColor = "#2060ff";
            var theme = new Theme(new[] { new SelectorBlock("Form", new[] { new Property("BackColor", htmlColor) }) });

            theme.ApplyTheme(form);

            Assert.Equal(ColorTranslator.FromHtml(htmlColor), form.BackColor);
        }

        [Fact]
        public void ThemeWithInlude()
        {
            IWinformThemeElement variables = new VariableBlock(new[] { new Property("colorOfSky", "Blue") });
            IWinformThemeElement selector = new SelectorBlock("Form", new[] { new Property("BackColor", "@colorOfSky") });
            var theme = new Theme(new[] { variables, selector });
            var theme2 = new Theme(new[] { new SelectorBlock("Button", new[] { new Property("BackColor", "Yellow") }) });
            theme.IncludeThemes.Add(theme2);
            var form = new TestForm { BackColor = Color.Black };
            form.button1.BackColor = Color.Black;
            form.button2.BackColor = Color.Black;

            theme.ApplyTheme(form);

            Assert.Equal(Color.Blue, form.BackColor);
            Assert.Equal(Color.Yellow, form.button1.BackColor);
            Assert.Equal(Color.Yellow, form.button2.BackColor);
        }
    }
}
