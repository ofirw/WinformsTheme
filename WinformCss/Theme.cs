using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinformsTheme.Elements;

namespace WinformsTheme
{
    public class Theme
    {
        public Theme(IEnumerable<IWinformThemeElement> themeElements)
        {
            this.Selectors = themeElements.OfType<SelectorBlock>();
            this.Variables = themeElements.OfType<VariableBlock>().DefaultIfEmpty(new VariableBlock()).Single();
        }

        public List<Theme> IncludeThemes { get; } = new List<Theme>();
        public IEnumerable<SelectorBlock> Selectors { get; }
        public VariableBlock Variables { get; }

        public void ApplyTheme(Control controlToApply) => this.ApplyTheme(controlToApply, this.Selectors);

        private void ApplyTheme(Control controlToApply, IEnumerable<SelectorBlock> selectors)
        {
            foreach (var selector in selectors)
            {
                foreach (var control in selector.SelectControls(controlToApply))
                {
                    SetProperties(control, selector);
                    this.ApplyTheme(control, selector.Selectors);
                }
            }
        }

        private void SetProperties(Control control, SelectorBlock selector)
        {
            foreach (var property in selector.Properties)
            {
                var reflectedProperty = StyledReflectedProperty.Create(control, property.Identifier);
                reflectedProperty.SetValue(property.GetValueAndApplyVariables(this.Variables));
            }
        }
    }
}