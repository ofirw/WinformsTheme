using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsTheme;

namespace DemoApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var theme = WinformsThemeLoader.Load("sample.theme");
            using (ThemeHooker.HookTheme(theme))
            {
                Application.Run(new Form1());
            }
        }
    }
}
