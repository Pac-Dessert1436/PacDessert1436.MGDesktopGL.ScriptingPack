using MessageBox = System.Windows.Forms.MessageBox;

namespace MonoGameJScript;

/// <summary>
/// Game host that exposes API to JScript engine.
/// </summary>
public sealed class GameHost : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private JScriptEngine? _jsEngine;

    public GameHost()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    #region Public API Methods
    public void SetWindowSize(int width, int height)
    {
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.ApplyChanges();
    }
    public int ViewportWidth => GraphicsDevice.Viewport.Width;
    public int ViewportHeight => GraphicsDevice.Viewport.Height;
    public SpriteBatch SpriteBatch => _spriteBatch!;
    #endregion

    protected override void Initialize()
    {
        // Initialize JScript engine, and expose API in LoadContent()
        _jsEngine = new JScriptEngine();
        MediaPlayer.IsRepeating = true;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        ArgumentNullException.ThrowIfNull(_jsEngine);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _jsEngine.AddHostObject("game", this);
        _jsEngine.LoadRequiredContent(Content);

        _jsEngine.Execute(File.ReadAllText("GameCore.js"));
        try
        {
            _jsEngine.Invoke("Initialize");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error in script Initialize: {ex.Message}");
            Exit();
        }

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        // Call script Update function
        try
        {
            _jsEngine?.Invoke("Update", gameTime.ElapsedGameTime.TotalSeconds);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error in script Update: {ex.Message}");
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Call script Draw function
        try
        {
            _jsEngine?.Invoke("Draw", gameTime.ElapsedGameTime.TotalSeconds);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error in script Draw: {ex.Message}");
            Exit();
        }

        base.Draw(gameTime);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _spriteBatch?.Dispose();
            _jsEngine?.Dispose();
        }

        base.Dispose(disposing);
    }

    private static void Main()
    {
        using GameHost host = new();
        host.Run();
    }
}