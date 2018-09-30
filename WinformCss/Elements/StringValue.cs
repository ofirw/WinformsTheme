using System.Collections.Generic;

namespace WinformsTheme.Elements
{

    public class StringValue : IWinformThemeElement
    {
        public StringValue(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}