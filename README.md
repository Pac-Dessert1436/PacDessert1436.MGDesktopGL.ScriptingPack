# MonoGame DesktopGL Scripting Pack

> **📢 DEVELOPMENT UPDATE**: The current version (1.1.0) adds C++/CLI support. This is a personal side project, and I am currently preparing for my postgraduate entrance exam, so **active development will be paused until December 21, 2026**.
>
> Thank you for your understanding and support. I look forward to continuing work on this project once my exams are complete! 🎓

This package provides a set of MonoGame DesktopGL 2D game templates with **JScript scripting support via a C# host**, **VBScript scripting support via a VB.NET host**, and a new **C++/CLI template** for developers who want a C++-style workflow inside the .NET ecosystem. Each template includes a complete Snake game demo to help you get started quickly.

All assets included in this package come with clear licensing terms and attribution requirements. See [Asset Credits](#asset-credits) for complete details on usage rights and restrictions.

_**Note**: This template package is intended for Windows systems; Linux and macOS are not currently supported. The project is licensed under the MIT License, and the NuGet package metadata has been corrected accordingly._

## VBScript Support: Preserving a Legacy

As the creator of **vbs-revive** (https://github.com/Pac-Dessert1436/vbs-revive/), a modern VBScript game engine built with VB.NET WinForms, I am deeply committed to preserving the ecosystems of both VBScript and VB.NET. Although Microsoft has announced plans to deprecate VBScript by 2027, this MonoGame scripting pack continues to offer full VBScript support as a tribute to the language’s legacy and its dedicated community.

For developers who want to keep using VBScript in game development, **vbs-revive** offers a forward-looking solution that preserves the familiar syntax while leveraging modern .NET capabilities. _For those who prefer a more future-proof option, the JScript and C++/CLI templates provide robust, well-supported scripting environments with similar flexibility._

## Features

- **JScript Template**: Full MonoGame DesktopGL project with a C# host and JScript scripting support
- **VBScript Template**: Full MonoGame DesktopGL project with a VB.NET host and VBScript scripting support
- **C++/CLI Template**: Full MonoGame DesktopGL project with a C# host and a C++/CLI core for a C++-style development experience
- **ClearScript Integration**: Seamless .NET and JScript/VBScript interoperability
- **Ready to Use**: Pre-configured with a content pipeline and a basic game structure

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- MonoGame 3.8 or later
- ClearScript (automatically included via NuGet)
- For the C++/CLI template, a Windows environment with Visual C++/CLI build support is recommended

## Installation

### Install from NuGet
```bash
dotnet new install PacDessert1436.MGDesktopGL.ScriptingPack
```

### Manual Installation
1. Clone or download this repository:
```bash
git clone https://github.com/Pac-Dessert1436/PacDessert1436.MGDesktopGL.ScriptingPack.git
```
2. Create the NuGet package:
```bash
nuget pack PacDessert1436.MGDesktopGL.ScriptingPack.nuspec
```
3. Install the template:
```bash
dotnet new install .
```

## Usage

### Create a JScript Game
```bash
dotnet new mg2djscript -n MyJScriptGame
cd MyJScriptGame
dotnet run
```

### Create a VBScript Game
```bash
dotnet new mg2dvbscript -n MyVBScriptGame
cd MyVBScriptGame
dotnet run
```

### Create a C++/CLI Game
```bash
dotnet new mg2dcppcli -n MyCppCliGame
cd MyCppCliGame
dotnet run
```

## Project Structure

### C# Host (GameHost.cs)
The C# host initializes MonoGame and sets up the **JScript** scripting environment:
- Loads and executes the script file
- Exposes MonoGame API to the scripting environment
- Handles game loop and input processing
- Core script file: `GameCore.js`

### VB.NET Host (GameHost.vb)
The VB.NET host initializes MonoGame and sets up the **VBScript** scripting environment:
- Loads and executes the script file
- Exposes MonoGame API to the scripting environment
- Handles game loop and input processing
- Core script file: `GameCore.vbs`

### C++/CLI Host and Core
The C++/CLI template uses a .NET host project together with a **C++/CLI core** project:
- The host initializes MonoGame and launches the game
- The C++/CLI core contains the game implementation and entry point
- The template is designed for developers who want a C++-style workflow while still using the .NET and MonoGame ecosystem
- Core entry point: `GameMain.cpp`

Both JScript and VBScript scripts contain the game logic:
- `Initialize()` - Called once at startup
- `Update(gameTime)` - Called every frame for game logic
- `Draw(gameTime)` - Called every frame for rendering

## Comprehensive Scripting API

Both templates provide access to a rich set of MonoGame types and framework-specific helpers:

| Category | Key Types | Namespace |
|----------|-----------|-----------|
| Core Framework | `GameTime`, `Color`, `Vector2`, `Point`, `Rectangle` | Microsoft.Xna.Framework |
| Graphics | `SpriteBatch`, `Texture2D`, `SpriteFont`, `Song`, `SoundEffect` | Microsoft.Xna.Framework.Graphics |
| Input | `Keyboard`, `Mouse`, `GamePad`, `Keys`, `ButtonState` | Microsoft.Xna.Framework.Input |
| Content Management | `ContentManager` | Microsoft.Xna.Framework.Content |
| Utilities | `MathHelper`, `MediaPlayer`, `Random` (JScript), `VBMath` (VBScript) | Various |

### API Documentation
Each template includes practical documentation to help you get started:
- Comprehensive type references
- Language-specific syntax examples
- Step-by-step usage guides
- Content customization instructions
- Best practices for game development

For the C++/CLI template, the main guide is [MonoGameCppCLI/CRASH_COURSE.md](MonoGameCppCLI/CRASH_COURSE.md), providing a full beginner-friendly introduction to C++/CLI syntax and patterns. The JScript and VBScript templates continue to use `API_CHEATSHEET.md` for detailed API references.

## Language-Specific Features

### JScript Template
- **Modern Syntax**: Uses standard JavaScript-style syntax with `function` declarations and `new` keyword for object creation
- **Case Sensitivity**: Follows JavaScript conventions with case-sensitive identifiers
- **Direct Object Creation**: No factory methods required - create objects directly using `new Vector2()`, `new Point()`, etc.
- **Random Numbers**: Uses `Random.Shared.Next()` for robust random number generation

### VBScript Template
- **Legacy Compatibility**: Maintains familiar VBScript syntax with `Sub`/`Function` declarations and the `Set` keyword
- **Case Insensitivity**: Follows VBScript conventions with case-insensitive identifiers
- **Factory Methods**: Uses `Helpers.CreateVector2()`, `Helpers.CreatePoint()`, and similar helpers, since VBScript does not support `new` for .NET objects
- **Random Numbers**: Uses `VBMath.Rnd()` and `VBMath.Randomize()` for traditional VBScript-style randomization

### C++/CLI Template
- **C++-Style Syntax**: Uses a C++/CLI code structure for developers who prefer a C++-style workflow
- **Native Feeling**: Offers a familiar syntax for those coming from C++ while still targeting the .NET runtime
- **MonoGame Integration**: Works with the same MonoGame API surface as the other templates

## Example Workflow

Both templates follow the same game loop pattern with language-specific syntax:

### JScript
```javascript
function Initialize() {
    game.SetWindowSize(800, 600);
    // Initialize game state
}

function Update(dt) {
    var keyState = Keyboard.GetState();
    // Handle input and update game logic
}

function Draw(dt) {
    game.GraphicsDevice.Clear(Color.Black);
    var batch = game.SpriteBatch;
    batch.Begin();
    // Render game graphics
    batch.End();
}
```

### VBScript
```vbscript
Sub Initialize
    game.SetWindowSize 800, 600
    ' Initialize game state
End Sub

Sub Update(dt)
    Dim keyState
    Set keyState = Keyboard.GetState()
    ' Handle input and update game logic
End Sub

Sub Draw(dt)
    game.GraphicsDevice.Clear Color.Black
    Dim batch
    Set batch = game.SpriteBatch
    batch.Begin
    ' Render game graphics
    batch.End
End Sub
```

### Full Demo Implementation
Both templates include a complete Snake game demo with:
- Responsive keyboard controls (WASD/arrow keys)
- Collision detection and game over state
- Score tracking and progressive difficulty
- Sound effects and background music
- Texture rendering and UI text

## Asset Credits

- **Graphics**: Original artwork created using Aseprite exclusively for this project. **NON-COMMERCIAL USE ONLY** - These image assets may not be used, modified, or distributed for any commercial purpose. _Non-commercial personal and educational use is permitted._
- **Music**: _Pixelated Hearts Parade_, from [10-track Modern Chiptune Demo](https://opengameart.org/content/10-track-modern-chiptune-demo) by IndieDevs, licensed under the CC0 License.
- **Sound Effects**: [512 Sound Effects 8-bit Style](https://opengameart.org/content/512-sound-effects-8-bit-style) by SubspaceAudio, licensed under the CC0 License.
- **Font**: [Fusion Pixel Font](https://github.com/TakWolf/fusion-pixel-font) by TakWolf, used with permission under SIL Open Font License.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
