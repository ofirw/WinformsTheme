using System.Collections.Generic;
using System.Linq;

namespace WinformsTheme.Elements
{
    public class WinformThemeBlock<T> : IWinformThemeElement where T : IWinformThemeElement
    {
        public WinformThemeBlock(string identifier, IEnumerable<T> attributes)
        {
            this.Identifier = identifier;
            this.ElementsDictionary = attributes.ToDictionary(e=>e.Identifier, e=>e);
        }

        public string Identifier { get; set; }
        public Dictionary<string, T> ElementsDictionary { get; set; }
    }
}