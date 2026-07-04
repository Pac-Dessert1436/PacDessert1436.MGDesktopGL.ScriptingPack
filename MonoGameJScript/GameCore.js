var CELL_SIZE = 15;
var GRID_WIDTH = 50;
var GRID_HEIGHT = 36;
var BASE_MOVE_INTERVAL = 0.15;
var MIN_MOVE_INTERVAL = 0.05;
var SPEED_INCREASE = 0.005;

var snake, snakeDir, food, score, gState, moveTimer;

function Initialize() {
    game.SetWindowSize(800, 600);
    gState = GameState.Title;
}

function ResetGame() {
    MediaPlayer.Play(bgmMainTheme);
    snake = new ArrayList();
    snakeDir = new Point(1, 0);
    score = 0;
    gState = GameState.Playing;
    moveTimer = 0;

    var startX = GRID_WIDTH / 2;
    var startY = GRID_HEIGHT / 2;
    for (var i = 0; i < 5; i++) {
        snake.Add(new Point(startX - i, startY));
    }
    SpawnFood();
}

function SpawnFood() {
    do {
        var newFoodX = Random.Shared.Next(0, GRID_WIDTH - 1);
        var newFoodY = Random.Shared.Next(0, GRID_HEIGHT - 1);
        var isOnSnake = false;

        for (var i = 0; i < snake.Count; i++) {
            var segment = snake[i];
            if (segment.X == newFoodX && segment.Y == newFoodY) {
                isOnSnake = true;
                break;
            }
        }
    } while (isOnSnake);

    food = new Point(newFoodX, newFoodY);
}

function Update(dt) {
    var keyState = Keyboard.GetState();

    if (gState.Equals(GameState.Title) || gState.Equals(GameState.GameOver)) {
        if (keyState.IsKeyDown(Keys.Space)) ResetGame();
        return;
    }
    else if (gState.Equals(GameState.Paused)) {
        if (keyState.IsKeyDown(Keys.Space)) {
            gState = GameState.Playing;
            MediaPlayer.Resume();
        }
        return;
    }
    HandleInput();

    interval = BASE_MOVE_INTERVAL - (snake.Count - 3) * SPEED_INCREASE;
    if (interval < MIN_MOVE_INTERVAL) interval = MIN_MOVE_INTERVAL;
    moveTimer = moveTimer + dt;
    if (moveTimer >= interval) {
        moveTimer = moveTimer - interval;
        var newHead = new Point(snake[0].X + snakeDir.X, snake[0].Y + snakeDir.Y);

        if (newHead.X < 0 || newHead.X >= GRID_WIDTH ||
            newHead.Y < 0 || newHead.Y >= GRID_HEIGHT) {
            TriggerGameOver();
            return;
        }
        for (var i = 0; i < snake.Count; i++) {
            var body = snake[i];
            if (body.X == newHead.X && body.Y == newHead.Y) {
                TriggerGameOver();
                return;
            }
        }

        snake.Insert(0, newHead);
        if (newHead.X == food.X && newHead.Y == food.Y) {
            score = score + 10;
            sndFood.Play();
            SpawnFood();
        } else {
            snake.RemoveAt(snake.Count - 1);
        }
    }
}

function HandleInput() {
    var newDir = snakeDir;

    var keyState = Keyboard.GetState();
    if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A)) {
        newDir = new Point(-1, 0);
    } else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D)) {
        newDir = new Point(1, 0);
    } else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W)) {
        newDir = new Point(0, -1);
    } else if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S)) {
        newDir = new Point(0, 1);
    } else if (keyState.IsKeyDown(Keys.P)) {
        gState = GameState.Paused;
        MediaPlayer.Pause();
    }

    if (newDir.X + snakeDir.X != 0 && newDir.Y + snakeDir.Y != 0) {
        snakeDir = newDir;
    }
}

function TriggerGameOver() {
    gState = GameState.GameOver;
    sndDeath.Play();
    MediaPlayer.Stop();
}

function DrawCenteredText(batch, text, y, color, textScale) {
    var textSize = font.MeasureString(text);
    var origin = new Vector2(textSize.X / 2, textSize.Y / 2);
    var position = new Vector2(game.ViewportWidth / 2, y);
    batch.DrawString(font, text, position, color, 0, origin, textScale, 0, 0);
}

function Draw(dt) {
    game.GraphicsDevice.Clear(Color.Black);
    var batch = game.SpriteBatch;
    batch.Begin();

    if (gState.Equals(GameState.Title)) {
        DrawCenteredText(batch, "SNAKE GAME", 250, Color.LightGreen, 2);
        DrawCenteredText(batch, "Use arrow keys to move", 320, Color.White, 1);
        DrawCenteredText(batch, "Press SPACE to start", 370, Color.Yellow, 1);
    } else if (gState.Equals(GameState.GameOver)) {
        DrawCenteredText(batch, "GAME OVER!", 250, Color.Red, 2);
        DrawCenteredText(batch, "Final Score: " + score, 320, Color.White, 1);
        DrawCenteredText(batch, "Press SPACE to restart", 370, Color.Yellow, 1);
    } else {
        batch.FillRectangle(0, 0, GRID_WIDTH * CELL_SIZE, GRID_HEIGHT * CELL_SIZE, Color.DarkCyan);
        for (var i = snake.Count - 1; i >= 0; i--) {
            var snakeBodyPos = new Vector2(snake[i].X * CELL_SIZE, snake[i].Y * CELL_SIZE);
            if (i === 0) {
                batch.Draw(imgSnakeHead, snakeBodyPos, Color.White);
            } else {
                batch.Draw(imgSnakeBody, snakeBodyPos, Color.White);
            }
        }
        var drawPos = new Vector2(food.X * CELL_SIZE, food.Y * CELL_SIZE);
        batch.Draw(imgRedApple, drawPos, Color.White);
        var drawPos = new Vector2(10, GRID_HEIGHT * CELL_SIZE + 10);
        batch.DrawString(font, "Score: " + score, drawPos, Color.White);

        if (gState.Equals(GameState.Paused)) {
            DrawCenteredText(batch, "- PAUSED -", 250, Color.Wheat, 2);
            DrawCenteredText(batch, "Press SPACE to resume", 320, Color.White, 1);
        }
    }
    batch.End();
}