Option Explicit
Const CELL_SIZE = 15
Const GRID_WIDTH = 50
Const GRID_HEIGHT = 36
Const BASE_MOVE_INTERVAL = 0.15 
Const MIN_MOVE_INTERVAL = 0.05  
Const SPEED_INCREASE = 0.005

Dim snake, snakeDir, food, score, gState, moveTimer

Sub Initialize()
    game.SetWindowSize 800, 600
    VBMath.Randomize
    gState = GameState.Title
End Sub

Sub ResetGame()
    MediaPlayer.Play bgmMainTheme
    snake = Helpers.CreateArrayList()
    snakeDir = Helpers.CreatePoint(1, 0)
    score = 0
    gState = GameState.Playing
    moveTimer = 0
    
    Dim startX, startY, i
    startX = GRID_WIDTH \ 2
    startY = GRID_HEIGHT \ 2
    For i = 0 To 4
        snake.Add Helpers.CreatePoint(startX - i, startY)
    Next
    
    SpawnFood
End Sub

Sub SpawnFood()
    Dim newFoodX, newFoodY, isOnSnake, i
    Do
        newFoodX = Convert.ToInt32(VBMath.Rnd() * (GRID_WIDTH - 1))
        newFoodY = Convert.ToInt32(VBMath.Rnd() * (GRID_HEIGHT - 1))
        isOnSnake = False
        
        For i = 0 To snake.Count - 1
            With snake(i)
                If .X = newFoodX And .Y = newFoodY Then
                    isOnSnake = True
                    Exit For
                End If
            End With
        Next
    Loop While isOnSnake
    
    Set food = Helpers.CreatePoint(newFoodX, newFoodY)
End Sub

Sub Update(dt)
    Dim i, body, newHead, interval, keyState
    Set keyState = Keyboard.GetState()

    If gState.Equals(GameState.Title) Or gState.Equals(GameState.GameOver) Then
        If keyState.IsKeyDown(Keys.Space) Then ResetGame
        Exit Sub
    ElseIf gState.Equals(GameState.Paused) Then
        If keyState.IsKeyDown(Keys.Space) Then 
            gState = GameState.Playing
            MediaPlayer.Resume
        End If
        Exit Sub
    End If
    HandleInput

    interval = BASE_MOVE_INTERVAL - (snake.Count - 3) * SPEED_INCREASE
    If interval < MIN_MOVE_INTERVAL Then interval = MIN_MOVE_INTERVAL
    moveTimer = moveTimer + dt
    If moveTimer >= interval Then
        moveTimer = moveTimer - interval
        With snake(0)
            Set newHead = Helpers.CreatePoint(.X + snakeDir.X, .Y + snakeDir.Y)
        End With

        If newHead.X < 0 Or newHead.X >= GRID_WIDTH Or _
            newHead.Y < 0 Or newHead.Y >= GRID_HEIGHT Then
            TriggerGameOver
            Exit Sub
        End If
        For i = 0 To snake.Count - 1
            Set body = snake(i)
            If body.X = newHead.X And body.Y = newHead.Y Then
                TriggerGameOver
                Exit Sub
            End If
        Next
        
        snake.Insert 0, newHead
        If newHead.X = food.X And newHead.Y = food.Y Then
            score = score + 10
            sndFood.Play
            SpawnFood
        Else
            snake.RemoveAt snake.Count - 1
        End If
    End If
End Sub

Sub HandleInput()
    Dim newDir
    Set newDir = snakeDir
    
    With Keyboard.GetState()
        If .IsKeyDown(Keys.Left) Or .IsKeyDown(Keys.A) Then
            Set newDir = Helpers.CreatePoint(-1, 0)
        ElseIf .IsKeyDown(Keys.Right) Or .IsKeyDown(Keys.D) Then
            Set newDir = Helpers.CreatePoint(1, 0)
        ElseIf .IsKeyDown(Keys.Up) Or .IsKeyDown(Keys.W) Then
            Set newDir = Helpers.CreatePoint(0, -1)
        ElseIf .IsKeyDown(Keys.Down) Or .IsKeyDown(Keys.S) Then
            Set newDir = Helpers.CreatePoint(0, 1)
        ElseIf .IsKeyDown(Keys.P) Then 
            gState = GameState.Paused
            MediaPlayer.Pause
        End If
    End With
    
    If (newDir.X + snakeDir.X <> 0) Or (newDir.Y + snakeDir.Y <> 0) Then
        Set snakeDir = newDir
    End If
End Sub

Sub TriggerGameOver()
    gState = GameState.GameOver
    sndDeath.Play
    MediaPlayer.Stop
End Sub

Sub DrawCenteredText(batch, text, y, color, textScale)
    Dim textSize, origin, position
    Set textSize = font.MeasureString(text)
    Set origin = Helpers.CreateVector2(textSize.X / 2, textSize.Y / 2)
    Set position = Helpers.CreateVector2(game.ViewportWidth / 2, y)
    batch.DrawString font, text, position, color, 0, origin, textScale, 0, 0
End Sub

Sub Draw(dt)
    Dim i, snakeBodyPos, batch, drawPos
    game.GraphicsDevice.Clear Color.Black
    Set batch = game.SpriteBatch
    batch.Begin
    
    If gState.Equals(GameState.Title) Then
        DrawCenteredText batch, "SNAKE GAME", 250, Color.LightGreen, 2
        DrawCenteredText batch, "Use arrow keys to move", 320, Color.White, 1
        DrawCenteredText batch, "Press SPACE to start", 370, Color.Yellow, 1
    ElseIf gState.Equals(GameState.GameOver) Then
        DrawCenteredText batch, "GAME OVER!", 250, Color.Red, 2
        DrawCenteredText batch, "Final Score: " & score, 320, Color.White, 1
        DrawCenteredText batch, "Press SPACE to restart", 370, Color.Yellow, 1
    Else
        batch.FillRectangle 0, 0, GRID_WIDTH * CELL_SIZE, GRID_HEIGHT * CELL_SIZE, Color.DarkCyan
        For i = snake.Count - 1 To 0 Step -1
            With snake(i)
                Set snakeBodyPos = Helpers.CreateVector2(.X * CELL_SIZE, .Y * CELL_SIZE)
            End With
            If i = 0 Then
                batch.Draw imgSnakeHead, snakeBodyPos, Color.White
            Else
                batch.Draw imgSnakeBody, snakeBodyPos, Color.White
            End If
        Next
        Set drawPos = Helpers.CreateVector2(food.X * CELL_SIZE, food.Y * CELL_SIZE)
        batch.Draw imgRedApple, drawPos, Color.White
        Set drawPos = Helpers.CreateVector2(10, GRID_HEIGHT * CELL_SIZE + 10)
        batch.DrawString font, "Score: " & score, drawPos, Color.White

        If gState.Equals(GameState.Paused) Then
            DrawCenteredText batch, "- PAUSED -", 250, Color.White, 2
            DrawCenteredText batch, "Press SPACE to resume", 320, Color.White, 1
        End If
    End If
    batch.End
End Sub