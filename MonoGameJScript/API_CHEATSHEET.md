# API Cheat Sheet for JScript Scripting in MonoGame

## Overview
This cheat sheet documents the API exposed to JScript scripts in the MonoGame framework. All these APIs are available globally in your JScript code.

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
```js
game.SetWindowSize(width, height);
// Sets the game window size (call in Initialize)
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

To customize your asset loading, you will need to modify the `Helpers.LoadRequiredContent` method in your game host (C# project; see [Helpers.cs](./Helpers.cs)).

## 3. Helper Methods (`Helpers` class)

### Drawing Helpers
```js
// Fill a rectangle with color (extension method on SpriteBatch)
spriteBatch.FillRectangle(x, y, width, height, color);
```

## 4. Input Handling

### Keyboard
```js
// Get current keyboard state
var keyState = Keyboard.GetState();

// Check if a key is pressed
if (keyState.IsKeyDown(Keys.Space)) {
    // Do something
}
```

**Common Key Constants**: `Keys.Up`, `Keys.Down`, `Keys.Left`, `Keys.Right`, `Keys.Space`, `Keys.Escape`, `Keys.W`, `Keys.A`, `Keys.S`, `Keys.D`, `Keys.P`

### Mouse
```js
// Get current mouse state
var mouseState = Mouse.GetState();

// Access mouse properties
var x = mouseState.X;
var y = mouseState.Y;
var leftButtonPressed = (mouseState.LeftButton === ButtonState.Pressed);
```

## 5. Media & Audio

### Background Music (MediaPlayer)
```js
// Play background music
MediaPlayer.Play(bgmMainTheme);

// Control playback
MediaPlayer.Pause();
MediaPlayer.Resume();
MediaPlayer.Stop();
MediaPlayer.IsRepeating = true;  // Enable loop
```

### Sound Effects
```js
// Play a sound effect
sndFood.Play();
sndDeath.Play();
```

## 6. Math & Utilities

### Random Numbers
```js
// Get random integer between min (inclusive) and max (exclusive)
var randomInt = Random.Shared.Next(min, max);

// Get random float between 0 and 1
var randomFloat = Math.random();
```

### MathHelper
```js
// Common math functions
var angleRadians = MathHelper.ToRadians(degrees);
var angleDegrees = MathHelper.ToDegrees(radians);
var clampedValue = MathHelper.Clamp(value, min, max);
var lerpedValue = MathHelper.Lerp(start, end, amount);
```

### Convert
```js
// Type conversion
var intValue = Convert.ToInt32(stringOrNumber);
var floatValue = Convert.ToDouble(stringOrNumber);
var stringValue = Convert.ToString(value);
```

## 7. Game State Management

### GameState Enum
```js
// Game state constants
GameState.Title;      // Title screen
GameState.Playing;    // Game in progress
GameState.Paused;     // Game paused
GameState.GameOver;   // Game over
```

## 8. Object Creation

JScript allows direct object creation using the `new` keyword:

```js
// Create a 2D vector
var vec = new Vector2(x, y);

// Create a point (integer coordinates)
var point = new Point(x, y);

// Create a color from RGB values (0-255)
var color = new Color(r, g, b);

// Create a rectangle
var rect = new Rectangle(x, y, width, height);

// Create an ArrayList (dynamic array)
var list = new ArrayList();
```

## 9. SpriteBatch Drawing

### Basic Drawing
```js
// Begin drawing
game.SpriteBatch.Begin();

// Draw texture at position
game.SpriteBatch.Draw(texture, position, color);

// Draw text
game.SpriteBatch.DrawString(font, text, position, color);

// End drawing
game.SpriteBatch.End();
```

### Advanced Text Drawing
```js
// Measure text size
var textSize = font.MeasureString(text);

// Draw centered text (example from sample code)
function DrawCenteredText(batch, text, y, color, textScale) {
    var textSize = font.MeasureString(text);
    var origin = new Vector2(textSize.X / 2, textSize.Y / 2);
    var position = new Vector2(game.ViewportWidth / 2, y);
    batch.DrawString(font, text, position, color, 0, origin, textScale, 0, 0);
}
```

## 10. Script Entry Points

Your JScript file must implement these mandatory functions:

```js
// Called once at game start
function Initialize() {
    // Initialize game state, set window size, etc.
}

// Called every frame with delta time (seconds)
function Update(dt) {
    // Handle input, update game logic
}

// Called every frame for drawing
function Draw(dt) {
    // Render game graphics
}
```

## 11. Example Usage

```js
function Initialize() {
    game.SetWindowSize(800, 600);
}

function Update(dt) {
    var keyState = Keyboard.GetState();
    
    if (keyState.IsKeyDown(Keys.Space)) {
        sndFood.Play();
    }
}

function Draw(dt) {
    game.GraphicsDevice.Clear(Color.Black);
    
    var batch = game.SpriteBatch;
    batch.Begin();
    
    // Draw centered text
    var textSize = font.MeasureString("Hello, MonoGame!");
    var position = new Vector2(
        (game.ViewportWidth - textSize.X) / 2,
        (game.ViewportHeight - textSize.Y) / 2
    );
    batch.DrawString(font, "Hello, MonoGame!", position, Color.White);
    
    batch.End();
}
```

---

## Notes
- JScript supports the `new` keyword for direct object creation unlike VBScript
- All API members are case-sensitive in JScript
- Refer to the sample `GameCore.js` file for complete usage examples
- Customize content loading by modifying `Helpers.LoadRequiredContent` in the C# project