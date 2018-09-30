using System.Windows.Forms;
using WinformsTheme.Util.Hook;

namespace WinformsTheme
{
    public static class ThemeHooker
    {
        private static Theme Theme;

        public static WindowsHooker HookTheme(Theme theme)
        {
            Theme = theme;
            return WindowsHooker.Hook(ApplyTheme);
        }

        private static void ApplyTheme(CWPRETSTRUCT msg)
        {
            try
            {
                if (msg.GetMessage() != WindowMessage.Create) return;
                if (!(Control.FromHandle(msg.hWnd) is Form form)) return;

                Theme.ApplyTheme(form);
            }
            catch
            { }
        }
    }
}
