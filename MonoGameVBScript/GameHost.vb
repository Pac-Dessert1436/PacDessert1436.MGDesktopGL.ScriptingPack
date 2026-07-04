' ===== NOTE ON GLOBAL IMPORTS =====
' Global imports for this hosting layer are defined in the VB.NET project file.
' See the `MonoGameVBScript.vbproj` file for details.

Public NotInheritable Class GameHost
    Inherits Game

    Private ReadOnly _graphics As GraphicsDeviceManager
    Private _spriteBatch As SpriteBatch
    Private _vbsEngine As VBScriptEngine

#Region "Public API Methods"
    Public Sub SetWindowSize(width As Integer, height As Integer)
        _graphics.PreferredBackBufferWidth = width
        _graphics.PreferredBackBufferHeight = height
        _graphics.ApplyChanges()
    End Sub

    Public ReadOnly Property ViewportWidth As Integer
        Get
            Return _graphics.PreferredBackBufferWidth
        End Get
    End Property

    Public ReadOnly Property ViewportHeight As Integer
        Get
            Return _graphics.PreferredBackBufferHeight
        End Get
    End Property

    Public ReadOnly Property SpriteBatch As SpriteBatch
        Get
            Return _spriteBatch
        End Get
    End Property
#End Region

    Private Sub New()
        _graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
        IsMouseVisible = True
    End Sub

    Protected Overrides Sub Initialize()
        ' Initialize VBScript engine, and expose API in LoadContent()
        _vbsEngine = New VBScriptEngine
        MediaPlayer.IsRepeating = True

        MyBase.Initialize()
    End Sub

    Protected Overrides Sub LoadContent()
        ArgumentNullException.ThrowIfNull(_vbsEngine)
        _spriteBatch = New SpriteBatch(GraphicsDevice)
        _vbsEngine.AddHostObject("game", Me)
        _vbsEngine.LoadRequiredContent(Content)
        _vbsEngine.Execute(IO.File.ReadAllText("GameCore.vbs"))
        Try
            _vbsEngine.Invoke("Initialize")
        Catch ex As Exception
            MsgBox($"Error in script Initialize: {ex.Message}")
            [Exit]()
        End Try

        MyBase.LoadContent()
    End Sub

    Protected Overrides Sub Update(gameTime As GameTime)
        If GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed OrElse
           Keyboard.GetState().IsKeyDown(Keys.Escape) Then [Exit]()

        ' Call script Update function
        Try
            _vbsEngine?.Invoke("Update", gameTime.ElapsedGameTime.TotalSeconds)
        Catch ex As Exception
            MsgBox($"Error in script Update: {ex.Message}")
            [Exit]()
        End Try

        MyBase.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(gameTime As GameTime)
        ' Call script Draw function
        Try
            _vbsEngine?.Invoke("Draw", gameTime.ElapsedGameTime.TotalSeconds)
        Catch ex As Exception
            MsgBox($"Error in script Draw: {ex.Message}")
            [Exit]()
        End Try

        MyBase.Draw(gameTime)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            _spriteBatch?.Dispose()
            _vbsEngine?.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Friend Shared Sub Main()
        Using host As New GameHost
            host.Run()
        End Using
    End Sub
End Class