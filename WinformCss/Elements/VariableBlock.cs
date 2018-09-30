using System.Collections.Generic;
using System.Linq;

namespace WinformsTheme.Elements
{
    public class VariableBlock : WinformThemeBlock<Property>
    {
        public VariableBlock() : this(Enumerable.Empty<Property>()) { }

        public VariableBlock(IEnumerable<Property> variables) : base("$Variables", variables)
        {
        }

        public bool TryGetValue(string key, out string variableValue)
        {
            variableValue = null;
            if (!this.ElementsDictionary.TryGetValue(key, out Property variable)) return false;

            variableValue = variable.GetValueAndApplyVariables(this);
            return true;
        }
    }
}