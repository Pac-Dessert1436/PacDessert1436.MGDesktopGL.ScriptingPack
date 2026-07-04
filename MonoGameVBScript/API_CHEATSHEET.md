# API Cheat Sheet for VBScript Scripting in MonoGame

## Overview
This cheat sheet documents the API exposed to VBScript scripts in the MonoGame framework. All these APIs are available globally in your VBScript code.

---

## 1. Core Game API (`game` object)

### Properties
| Property | Type | Description |
|----------|------|-------------|
| `ViewportWidth` | Integer | Gets the current window width |
| `ViewportHeight` | Integer | Gets the current window height |
| `SpriteBatch` | SpriteBatch | Gets the sprite batch for drawing |
| `GraphicsDevice` | GraphicsDevice | Gets the graphics device |

### Methods
```vbs
game.SetWindowSize(width, height)
' Sets the game window size (call in Initialize)
```

## 2. Content Assets (can be customized)

These global variables provide direct access to loaded content:

| Asset | Type | Description |
|-------|------|-------------|
| `font` | SpriteFont | Main game font (GameFont.spritefont) |
| `bgmMainTheme` | Song | Main background music (main_theme.ogg) |
| `sndFood` | SoundEffect | Food collection sound effect |
| `sndDeath` | SoundEffect | Game over sound effect |
| `imgRedApple` | Texture2D | Red apple food texture |
| `imgSnakeBody` | Texture2D | Snake body segment texture |
| `imgSnakeHead` | Texture2D | Snake head texture |

To customize your asset loading, you will need to modify the `Helpers.LoadRequiredContent` subroutine in your game host (VB.NET project; see [Helpers.vb](./Helpers.vb)).

## 3. Helper Methods (`Helpers` module)

### Object Creation
```vbs
' Create a 2D vector
Set vec = Helpers.CreateVector2(x, y)

' Create a point (integer coordinates)
Set point = Helpers.CreatePoint(x, y)

' Create a color from RGB values (0-255)
Set color = Helpers.CreateColor(r, g, b)

' Create a rectangle
Set rect = Helpers.CreateRectangle(x, y, width, height)

' Create an ArrayList (dynamic array)
Set list = Helpers.CreateArrayList()
```

### Drawing Helpers
```vbs
' Fill a rectangle with color (extension method on SpriteBatch)
spriteBatch.FillRectangle(x, y, width, height, color)
```

## 4. Input Handling

### Keyboard
```vbs
' Get current keyboard state
Set keyState = Keyboard.GetState()

' Check if a key is pressed
If keyState.IsKeyDown(Keys.Space) Then
    ' Do something
End If
```

**Common Key Constants**: `Keys.Up`, `Keys.Down`, `Keys.Left`, `Keys.Right`, `Keys.Space`, `Keys.Escape`, `Keys.W`, `Keys.A`, `Keys.S`, `Keys.D`, `Keys.P`

### Mouse
```vbs
' Get current mouse state
Set mouseState = Mouse.GetState()

' Access mouse properties
x = mouseState.X
y = mouseState.Y
leftButtonPressed = (mouseState.LeftButton = ButtonState.Pressed)
```

## 5. Media & Audio

### Background Music (MediaPlayer)
```vbs
' Play background music
MediaPlayer.Play(bgmMainTheme)

' Control playback
MediaPlayer.Pause()
MediaPlayer.Resume()
MediaPlayer.Stop()
MediaPlayer.IsRepeating = True  ' Enable loop
```

### Sound Effects
```vbs
' Play a sound effect
 sndFood.Play()
 sndDeath.Play()
```

## 6. Math & Utilities

### VBMath (Random Numbers)
```vbs
' Initialize random number generator
VBMath.Randomize

' Get random number between 0 and 1 (exclusive)
randomValue = VBMath.Rnd()

' Get random integer between min and max (inclusive)
randomInt = Convert.ToInt32(VBMath.Rnd() * (max - min) + min)
```

### MathHelper
```vbs
' Common math functions
angleRadians = MathHelper.ToRadians(degrees)
angleDegrees = MathHelper.ToDegrees(radians)
clampedValue = MathHelper.Clamp(value, min, max)
lerpedValue = MathHelper.Lerp(start, [end], amount)
```

### Convert
```vbs
' Type conversion
intValue = Convert.ToInt32(stringOrNumber)
floatValue = Convert.ToDouble(stringOrNumber)
stringValue = Convert.ToString(value)
```

## 7. Game State Management

### GameState Enum
```vbs
' Game state constants
GameState.Title      ' Title screen
GameState.Playing    ' Game in progress
GameState.Paused     ' Game paused
GameState.GameOver   ' Game over
```

## 8. SpriteBatch Drawing

### Basic Drawing
```vbs
' Begin drawing
game.SpriteBatch.Begin

' Draw texture at position
game.SpriteBatch.Draw(texture, position, color)

' Draw text
game.SpriteBatch.DrawString(font, text, position, color)

' End drawing
game.SpriteBatch.End
```

### Advanced Text Drawing
```vbs
' Measure text size
Set textSize = font.MeasureString(text)

' Draw centered text (example from sample code)
Sub DrawCenteredText(batch, text, y, color, textScale)
    Dim textSize, origin, position
    Set textSize = font.MeasureString(text)
    Set origin = Helpers.CreateVector2(textSize.X / 2, textSize.Y / 2)
    Set position = Helpers.CreateVector2(game.ViewportWidth / 2, y)
    batch.DrawString font, text, position, color, 0, origin, textScale, 0, 0
End Sub
```

## 9. Script Entry Points

Your VBScript file must implement these mandatory functions:

```vbs
' Called once at game start
Sub Initialize()
    ' Initialize game state, set window size, etc.
End Sub

' Called every frame with delta time (seconds)
Sub Update(dt)
    ' Handle input, update game logic
End Sub

' Called every frame for drawing
Sub Draw(dt)
    ' Render game graphics
End Sub
```

## 10. Example Usage

```vbs
Option Explicit
Dim keyState, batch

Sub Initialize()
    game.SetWindowSize(800, 600)
    VBMath.Randomize
End Sub

Sub Update(dt)
    Set keyState = Keyboard.GetState()
    If keyState.IsKeyDown(Keys.Space) Then sndFood.Play()
End Sub

Sub Draw(dt)
    game.GraphicsDevice.Clear(Color.Black)
    Set batch = game.SpriteBatch
    batch.Begin
    
    ' Draw centered text
    Dim textSize, position
    Set textSize = font.MeasureString("Hello, MonoGame!")
    Set position = Helpers.CreateVector2( _
        (game.ViewportWidth - textSize.X) / 2, _
        (game.ViewportHeight - textSize.Y) / 2 _
    )
    batch.DrawString font, "Hello, MonoGame!", position, Color.White
    
    batch.End
End Sub
```

---

## Notes
- VBScript does not support the `New` keyword for object creation - use the provided helper methods instead
- All API members are case-insensitive in VBScript
- Refer to the sample `GameCore.vbs` file for complete usage examples