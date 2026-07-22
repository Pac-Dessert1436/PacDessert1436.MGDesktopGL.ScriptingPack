' ===== NOTE ON GLOBAL IMPORTS =====
' Global imports for this hosting layer are defined in the VB.NET project file.
' See the `MonoGameVBScript.vbproj` file for details.

''' <summary>
''' Helper methods for creating common objects in VBScript.
''' </summary>
''' <remarks>
''' Note that VBScript does not support the `New` keyword for object creation.
''' </remarks>
Public Module Helpers
    ''' <summary>
    ''' Loads required content into the VBScript engine.
    ''' </summary>
    ''' <remarks>
    ''' This method preloads all core content assets and exposes them as global variables
    ''' in the VBScript environment. To customize your asset loading:
    ''' <list type="number">
    ''' <item><description>Add new content files to your Content project</description></item>
    ''' <item><description>Use the content.Load(Of T)("Path/To/Asset") pattern to load them</description></item>
    ''' <item><description>Expose them to scripts using .AddHostObject("assetName", loadedAsset)</description></item>
    ''' </list>
    ''' All exposed assets become global variables in VBScript with the names specified.
    ''' </remarks>
    ''' <param name="vbsEngine">The VBScript engine to load content into.</param>
    ''' <param name="content">Content manager to load content from.</param>
    <Runtime.CompilerServices.Extension>
    Friend Sub LoadRequiredContent(vbsEngine As VBScriptEngine, content As ContentManager)
        With vbsEngine
            .AddHostObject("font", content.Load(Of SpriteFont)("Fonts/GameFont"))
            .AddHostObject("bgmMainTheme", content.Load(Of Song)("Sounds/main_theme"))
            .AddHostObject("sndFood", content.Load(Of SoundEffect)("Sounds/food_sound"))
            .AddHostObject("sndDeath", content.Load(Of SoundEffect)("Sounds/death_sound"))
            .AddHostObject("imgRedApple", content.Load(Of Texture2D)("Images/red_apple"))
            .AddHostObject("imgSnakeBody", content.Load(Of Texture2D)("Images/snake_body"))
            .AddHostObject("imgSnakeHead", content.Load(Of Texture2D)("Images/snake_head"))

            ' NOTE: These host types are naturally added to the script engine *without*
            '       namespace conflicts because of the project-level imports.
            .AddHostTypes(
                GetType(ButtonState), GetType(Keys), GetType(MathHelper), GetType(Keyboard),
                GetType(Mouse), GetType(Convert), GetType(Helpers), GetType(VBMath),
                GetType(MediaPlayer), GetType(Color), GetType(Vector2), GetType(GameState)
            )
        End With
    End Sub

#Region "Common object creation functions"
    Public Function CreateVector2(x As Double, y As Double) As Vector2
        Return New Vector2(CSng(x), CSng(y))
    End Function

    Public Function CreatePoint(x As Integer, y As Integer) As Point
        Return New Point(x, y)
    End Function

    Public Function CreateColor(r As Integer, g As Integer, b As Integer) As Color
        Return New Color(r, g, b)
    End Function

    Public Function CreateRectangle _
            (x As Integer, y As Integer, width As Integer, height As Integer) As Rectangle
        Return New Rectangle(x, y, width, height)
    End Function

    Public Function CreateArrayList() As ArrayList
        Return New ArrayList
    End Function
#End Region

    <Runtime.CompilerServices.Extension>
    Public Sub FillRectangle(batch As SpriteBatch,
            x As Integer, y As Integer, width As Integer, height As Integer, color As Color)
        ' Stabilize this texture's state to avoid creating multiple textures
        Static pixel As New Texture2D(batch.GraphicsDevice, 1, 1)
        pixel.SetData({color})
        batch.Draw(pixel, New Rectangle(x, y, width, height), color)
    End Sub
End Module

Public Enum GameState As Integer
    Title = 0
    Playing = 1
    Paused = 2
    GameOver = 3
End Enum