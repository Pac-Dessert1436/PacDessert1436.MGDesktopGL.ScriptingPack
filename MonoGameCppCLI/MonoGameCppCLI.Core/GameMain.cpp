using namespace System;
using namespace System::Collections::Generic;
using namespace Microsoft::Xna::Framework;
using namespace Microsoft::Xna::Framework::Graphics;
using namespace Microsoft::Xna::Framework::Content;
using namespace Microsoft::Xna::Framework::Audio;
using namespace Microsoft::Xna::Framework::Media;
using namespace Microsoft::Xna::Framework::Input;

public enum class GameState {
	Title = 0,
	Playing = 1,
	Paused = 2,
	GameOver = 3
};

public ref class GameMain : public Game
{
private:
	GraphicsDeviceManager^ _graphics;
	SpriteBatch^ _spriteBatch;

	// Assets
	SpriteFont^ font;
	Song^ bgmMainTheme;
	SoundEffect^ sndFood;
	SoundEffect^ sndDeath;
	Texture2D^ imgRedApple;
	Texture2D^ imgSnakeBody;
	Texture2D^ imgSnakeHead;
	Texture2D^ _blankTexture;

	// Game state
	List<Point>^ snake;
	Point snakeDir;
	Point food;
	int score;
	GameState gState;

	double moveTimer;
	Random^ rng;

	const int CELL_SIZE = 15;
	const int GRID_WIDTH = 50;
	const int GRID_HEIGHT = 36;
	const double BASE_MOVE_INTERVAL = 0.15;
	const double MIN_MOVE_INTERVAL = 0.05;
	const double SPEED_INCREASE = 0.005;

public:
	GameMain()
	{
		_graphics = gcnew GraphicsDeviceManager(this);
		Content->RootDirectory = "./Content";
		IsMouseVisible = true;
	}

protected:
	virtual void Initialize() override
	{
		_graphics->PreferredBackBufferWidth = 800;
		_graphics->PreferredBackBufferHeight = 600;
		_graphics->ApplyChanges();

		rng = gcnew Random();
		gState = GameState::Title;
		MediaPlayer::IsRepeating = true;
		Game::Initialize();
	}

	virtual void LoadContent() override
	{
		_spriteBatch = gcnew SpriteBatch(GraphicsDevice);

		font = Content->Load<SpriteFont^>("Fonts/GameFont");
		bgmMainTheme = Content->Load<Song^>("Sounds/main_theme");
		sndFood = Content->Load<SoundEffect^>("Sounds/food_sound");
		sndDeath = Content->Load<SoundEffect^>("Sounds/death_sound");
		imgRedApple = Content->Load<Texture2D^>("Images/red_apple");
		imgSnakeBody = Content->Load<Texture2D^>("Images/snake_body");
		imgSnakeHead = Content->Load<Texture2D^>("Images/snake_head");

		_blankTexture = gcnew Texture2D(GraphicsDevice, 1, 1);
		array<Color>^ data = gcnew array<Color>(1) { Color::White };
		_blankTexture->SetData(data);
	}

	virtual void Update(GameTime^ gameTime) override
	{
		if (GamePad::GetState(PlayerIndex::One).Buttons.Back == ButtonState::Pressed ||
			Keyboard::GetState().IsKeyDown(Keys::Escape)) Exit();

		double dt = gameTime->ElapsedGameTime.TotalSeconds;

		if (gState == GameState::Title || gState == GameState::GameOver)
		{
			if (Keyboard::GetState().IsKeyDown(Keys::Space)) ResetGame();
			return;
		}
		else if (gState == GameState::Paused)
		{
			if (Keyboard::GetState().IsKeyDown(Keys::Space))
			{
				gState = GameState::Playing;
				MediaPlayer::Resume();
			}
			return;
		}

		HandleInput();

		double interval = BASE_MOVE_INTERVAL - (snake->Count - 3) * SPEED_INCREASE;
		if (interval < MIN_MOVE_INTERVAL) interval = MIN_MOVE_INTERVAL;
		moveTimer += dt;
		if (moveTimer >= interval)
		{
			moveTimer -= interval;
			Point head = snake[0];
			Point newHead(head.X + snakeDir.X, head.Y + snakeDir.Y);

			if (newHead.X < 0 || newHead.X >= GRID_WIDTH || newHead.Y < 0 || newHead.Y >= GRID_HEIGHT)
			{
				TriggerGameOver();
				return;
			}

			for each(Point body in snake)
			{
				if (body.X == newHead.X && body.Y == newHead.Y)
				{
					TriggerGameOver();
					return;
				}
			}

			snake->Insert(0, newHead);
			if (newHead.X == food.X && newHead.Y == food.Y)
			{
				score += 10;
				sndFood->Play();
				SpawnFood();
			}
			else
			{
				snake->RemoveAt(snake->Count - 1);
			}
		}

		Game::Update(gameTime);
	}

	virtual void Draw(GameTime^ gameTime) override
	{
		GraphicsDevice->Clear(Color::Black);
		_spriteBatch->Begin(
			SpriteSortMode::Deferred,
			BlendState::AlphaBlend,
			nullptr,                          // SamplerState^
			nullptr,                          // DepthStencilState^
			nullptr,                          // RasterizerState^
			nullptr,                          // Effect^
			System::Nullable<Matrix>()        // Nullable<Matrix>
		);

		if (gState == GameState::Title)
		{
			DrawCenteredText("SNAKE GAME", 250, Color::LightGreen, 2.0f);
			DrawCenteredText("Use arrow keys to move", 320, Color::White, 1.0f);
			DrawCenteredText("Press SPACE to start", 370, Color::Yellow, 1.0f);
		}
		else if (gState == GameState::GameOver)
		{
			DrawCenteredText("GAME OVER!", 250, Color::Red, 2.0f);
			DrawCenteredText(String::Format("Final Score: {0}", score), 320, Color::White, 1.0f);
			DrawCenteredText("Press SPACE to restart", 370, Color::Yellow, 1.0f);
		}
		else
		{
			// background grid area
			FillRectangle(0, 0, GRID_WIDTH * CELL_SIZE, GRID_HEIGHT * CELL_SIZE, Color::DarkCyan);

			for (int i = snake->Count - 1; i >= 0; --i)
			{
				Point p = snake[i];
				Vector2 pos((float)(p.X * CELL_SIZE), (float)(p.Y * CELL_SIZE));
				if (i == 0)
					_spriteBatch->Draw(imgSnakeHead, pos, Color::White);
				else
					_spriteBatch->Draw(imgSnakeBody, pos, Color::White);
			}

			Vector2 foodPos((float)(food.X * CELL_SIZE), (float)(food.Y * CELL_SIZE));
			_spriteBatch->Draw(imgRedApple, foodPos, Color::White);

			Vector2 scorePos(10.0f, (float)(GRID_HEIGHT * CELL_SIZE + 10));
			_spriteBatch->DrawString(font, String::Format("Score: {0}", score), scorePos, Color::White);

			if (gState == GameState::Paused)
			{
				DrawCenteredText("- PAUSED -", 250, Color::White, 2.0f);
				DrawCenteredText("Press SPACE to resume", 320, Color::White, 1.0f);
			}
		}

		_spriteBatch->End();

		Game::Draw(gameTime);
	}

private:
	void ResetGame()
	{
		MediaPlayer::Play(bgmMainTheme);
		snake = gcnew List<Point>();
		snakeDir = Point(1, 0);
		score = 0;
		gState = GameState::Playing;
		moveTimer = 0.0;

		int startX = GRID_WIDTH / 2;
		int startY = GRID_HEIGHT / 2;
		for (int i = 0; i <= 4; ++i)
		{
			snake->Add(Point(startX - i, startY));
		}

		SpawnFood();
	}

	void SpawnFood()
	{
		int newFoodX, newFoodY;
		bool isOnSnake;
		do
		{
			newFoodX = (int)(rng->NextDouble() * (GRID_WIDTH - 1));
			newFoodY = (int)(rng->NextDouble() * (GRID_HEIGHT - 1));
			isOnSnake = false;
			for each(Point body in snake)
			{
				if (body.X == newFoodX && body.Y == newFoodY) { isOnSnake = true; break; }
			}
		} while (isOnSnake);

		food = Point(newFoodX, newFoodY);
	}

	void HandleInput()
	{
		Point newDir = snakeDir;
		KeyboardState ks = Keyboard::GetState();
		if (ks.IsKeyDown(Keys::Left) || ks.IsKeyDown(Keys::A)) newDir = Point(-1, 0);
		else if (ks.IsKeyDown(Keys::Right) || ks.IsKeyDown(Keys::D)) newDir = Point(1, 0);
		else if (ks.IsKeyDown(Keys::Up) || ks.IsKeyDown(Keys::W)) newDir = Point(0, -1);
		else if (ks.IsKeyDown(Keys::Down) || ks.IsKeyDown(Keys::S)) newDir = Point(0, 1);
		else if (ks.IsKeyDown(Keys::P)) { gState = GameState::Paused; MediaPlayer::Pause(); }

		if ((newDir.X + snakeDir.X) != 0 || (newDir.Y + snakeDir.Y) != 0) snakeDir = newDir;
	}

	void TriggerGameOver()
	{
		gState = GameState::GameOver;
		sndDeath->Play();
		MediaPlayer::Stop();
	}

	void DrawCenteredText(String^ text, float y, Color color, float textScale)
	{
		Vector2 textSize = font->MeasureString(text);
		Vector2 origin(textSize.X / 2, textSize.Y / 2);
		Vector2 position((float)_graphics->PreferredBackBufferWidth / 2.0f, y);
		_spriteBatch->DrawString(font, text, position, color, 0.0f, origin, textScale, SpriteEffects::None, 0.0f);
	}

	void FillRectangle(int x, int y, int width, int height, Color color)
	{
		// reuse one-pixel texture with tint
		_spriteBatch->Draw(_blankTexture, Rectangle(x, y, width, height), color);
	}

public:
	static void Main(array<String^>^ args)
	{
		GameMain^ game = gcnew GameMain();
		game->Run();
	}
};