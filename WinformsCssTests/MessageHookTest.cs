using System.Drawing;
using System.Windows.Forms;
using WinformsTheme;
using WinformsTheme.Elements;
using Xunit;

namespace WinformsThemeTests
{
    public class MessageHookTest
    {
        [Fact]
        public void RegisterMessage()
        {
            var form = new TestForm();
            form.button1.BackColor = form.button2.BackColor = form.BackColor = Color.Black;
            var theme = new Theme(new[] { new SelectorBlock("Button", new[] { new Property("BackColor", nameof(Color.AliceBlue)) }) });

            using (ThemeHooker.HookTheme(theme))
            {
                var timer = new Timer();
                timer.Tick += (s, e) => form.Close();
                timer.Interval = 1000;
                timer.Start();
                Application.Run(form);
            }

            Assert.Equal(Color.AliceBlue, form.button1.BackColor);
            Assert.Equal(Color.AliceBlue, form.button2.BackColor);
            Assert.Equal(Color.Black, form.BackColor);
        }
    }
}
