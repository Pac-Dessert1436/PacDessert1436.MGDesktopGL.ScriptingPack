using System.Collections;

namespace MonoGameJScript;

/// <summary>
/// Helper methods for JScript scripting.
/// </summary>
/// <remarks>
/// Note that JScript does not require factory methods for creating Point, Rectangle, Color, 
/// etc., because they can be created directly in JScript using the `new` keyword.
/// </remarks>
public static class Helpers
{
    extension(JScriptEngine jsEngine)
    {
        /// <summary>
        /// Load required content into JScript engine.
        /// </summary>
        /// <remarks>
        /// This method preloads all core content assets and exposes them as global variables
        /// in the JScript environment. To customize your asset loading:
        /// <list type="number">
        /// <item><description>Add new content files to your Content project</description></item>
        /// <item><description>Use the content.Load<T>("Path/To/Asset") pattern to load them</description></item>
        /// <item><description>Expose them to scripts using .AddHostObject("assetName", loadedAsset)</description></item>
        /// </list>
        /// All exposed assets become global variables in JScript with the names specified.
        /// </remarks>
        /// <param name="content">Content manager to load content from.</param>
        internal void LoadRequiredContent(ContentManager content)
        {
            jsEngine.AddHostObject("font", content.Load<SpriteFont>("Fonts/GameFont"));
            jsEngine.AddHostObject("bgmMainTheme", content.Load<Song>("Sounds/main_theme"));
            jsEngine.AddHostObject("sndFood", content.Load<SoundEffect>("Sounds/food_sound"));
            jsEngine.AddHostObject("sndDeath", content.Load<SoundEffect>("Sounds/death_sound"));
            jsEngine.AddHostObject("imgRedApple", content.Load<Texture2D>("Images/red_apple"));
            jsEngine.AddHostObject("imgSnakeBody", content.Load<Texture2D>("Images/snake_body"));
            jsEngine.AddHostObject("imgSnakeHead", content.Load<Texture2D>("Images/snake_head"));

            jsEngine.AddHostTypes(
                typeof(Color), typeof(ButtonState), typeof(Keys), typeof(Rectangle),
                typeof(Point), typeof(MathHelper), typeof(Keyboard), typeof(Mouse),
                typeof(Vector2), typeof(Convert), typeof(Helpers), typeof(Random),
                typeof(GameState), typeof(ArrayList), typeof(MediaPlayer)
            );
        }
    }

    private static Texture2D? _pixel;  // stable pixel texture's state

    extension(SpriteBatch batch)
    {
        public void FillRectangle(int x, int y, int width, int height, Color color)
        {
            _pixel ??= new Texture2D(batch.GraphicsDevice, 1, 1);
            ArgumentNullException.ThrowIfNull(_pixel);
            Rectangle rect = new(x, y, width, height);
            _pixel?.SetData([color]);
            batch?.Draw(_pixel, rect, color);
        }
    }
}

public enum GameState : int
{
    Title = 0,
    Playing = 1,
    Paused = 2,
    GameOver = 3
}