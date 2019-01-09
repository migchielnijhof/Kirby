using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{
    public static GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    /// <summary>
    /// The amount of lives of the player.
    /// </summary>
    public static byte PlayerLives;

    /// <summary>
    /// The amount of lives the player starts with.
    /// </summary>
    const byte basePlayerLives = 4;

    // Textures.
    protected static Texture2D titleScreen;

    /// <summary>
    /// The current game state.
    /// </summary>
    public static GameStates GameState;

    /// <summary>
    /// All possible game states.
    /// </summary>
    public enum GameStates { TitleScreen, Playing, GameOver };
    
    private Input input;

    private Level level;

    public Game()
    {
        graphics = new GraphicsDeviceManager(this);
        PlayerLives = basePlayerLives;
        input = new Input();
        GameState = GameStates.TitleScreen;
        Content.RootDirectory = "Content";
        level = new Level(null);
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        graphics.PreferredBackBufferWidth = 500;
        graphics.PreferredBackBufferHeight = 500;
        graphics.ApplyChanges();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);

        titleScreen = Content.Load<Texture2D>("titleScreen");
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        switch (GameState)
        {
            case GameStates.Playing:
                level.Update(gameTime);
                break;
        }
    }

    /// <summary>
    /// Handles input from the user. This is called before updates in the game happen.
    /// </summary>
    void HandleInput()
    {
        input.Update();
        switch (GameState)
        {
            case GameStates.TitleScreen:
                if (input.Start)
                {
                    GameState = GameStates.Playing;
                    level.Load(1);
                }
                break;
            case GameStates.Playing:
                level.HandleInput(input);
                break;
        }
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();

        switch (GameState)
        {
            case GameStates.TitleScreen:
                spriteBatch.Draw(titleScreen, Vector2.Zero, Color.White);
                break;
            case GameStates.Playing:
                level.Draw(spriteBatch, gameTime);
                break;
        }

        spriteBatch.End();
    }

}
