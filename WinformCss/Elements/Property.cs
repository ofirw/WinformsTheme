namespace WinformsTheme.Elements
{

    public class Property : IWinformThemeElement
    {
        private readonly string value;

        public Property(string identifier, string value)
        {
            this.Identifier = identifier;
            this.value = value;
        }

        public string Identifier { get; set; }

        public string GetRawValue() => this.value;

        public string GetValueAndApplyVariables(VariableBlock variableBlock)
        {
            if (!this.value.StartsWith("@")) return this.value;

            variableBlock.TryGetValue(this.value.Substring(1), out string variableValue);

            return variableValue;
        }
    }
}