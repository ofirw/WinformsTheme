# WinformsTheme
Kind of CSS for Winforms. 

## Features
- Selectors by Type, Name and class (using `Tag`).
- Nested selectors.
- Supports all controls properties:
    - Set via Reflection
    - Set with Constructor
    - Supports `KnownColors` and `HtmlColors`
- Variables block definition.
- Can include parent theme files.
- Simple registration by windows hook

### Selector by type

    Button { ... }
    
### Selector by name

    #button1 { .... }
    
### Selector by class

     this.button1.Tag = "Btn"; // <-- in the code
    
    .Btn { ... }

### Nested selectors

    Panel
    {
        Button { ... }
    }

### Properties

    Button 
    {
        BackColor : Red; // KnownColor
        ForeColor : #67ffff; // HTML
        Font: Arial, 20, Bold; // By font ctor
    }
    
### Variable block

    $Variable
    {
        SomeColor: Blue;
    }

    Form 
    {
        BackColor : @SomeColor;
    }
    
### Include block

    $Include 
    {
        parent1.theme;
        parent2.theme;
    }
    
### Registration

    // Program.cs
    var theme = WinformsThemeLoader.Load("sample.theme");
    using (ThemeHooker.HookTheme(theme))
    {
        Application.Run(new Form1());
    }

----
See the Demo project for full working sample.

Any contribution is welcomed!

*Theme format inspired by https://github.com/paradoxlost/ux*
