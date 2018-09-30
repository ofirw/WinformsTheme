using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinformsTheme.Extensions;

namespace WinformsTheme.Elements
{

    public class SelectorBlock : WinformThemeBlock<IWinformThemeElement>
    {
        public SelectorBlock(string identifier, IEnumerable<IWinformThemeElement> attributes) : base(identifier, attributes)
        {
        }

        public IEnumerable<Property> Properties => this.ElementsDictionary.Values.OfType<Property>();
        public IEnumerable<SelectorBlock> Selectors => this.ElementsDictionary.Values.OfType<SelectorBlock>();

        public IEnumerable<Control> SelectControls(Control control)
        {
            if (this.Identifier.StartsWith("#"))
            {
                return control.DescendantsControlsAndSelf().Where(ctrl => ctrl.Name == this.Identifier.Substring(1));
            }
            else if (this.Identifier.StartsWith("."))
            {
                return control.DescendantsControlsAndSelf().Where(c => c.Tag?.ToString().Split().Contains(this.Identifier.Substring(1)) == true);
            }
            else
            {
                return control.DescendantsControlsAndSelf().Where(c => c.GetType().TypeOrBaseTypesNameIs(this.Identifier, "Control"));
            }
        }
    }
}