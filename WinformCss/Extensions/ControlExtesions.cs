using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinformsTheme.Extensions
{
    public static class ControlExtesions
    {
        public static IEnumerable<Control> DescendantsControls(this Control control)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(DescendantsControls).Concat(controls);
        }

        public static IEnumerable<Control> DescendantsControlsAndSelf(this Control control) =>
            control.DescendantsControls().Concat(new[] { control });
    }
}