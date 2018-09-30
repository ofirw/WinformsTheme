using System.Collections.Generic;

namespace WinformsTheme.Elements
{

    public class IncludeBlock : WinformThemeBlock<StringValue> {
        public IncludeBlock(IEnumerable<StringValue> attributes) : base("$Include", attributes)
        {
        }
    }
}