﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{
    public static GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public const float SpriteScale = 5;

    public const int ScreenWidth = 160;

    public const int ScreenHeight = 144;

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
    public enum GameStates : byte { TitleScreen, Playing, GameOver };
    
    private Input input;

    private Level level;

    private const string tileDirectory = "Tiles/";

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
        graphics.PreferredBackBufferWidth = (int) (ScreenWidth * SpriteScale);
        graphics.PreferredBackBufferHeight = (int) (ScreenHeight * SpriteScale);
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

        for (byte x = 0; x <= Tile.MaxID; x++)
            Tile.sprites[x] = Content.Load<Texture2D>(tileDirectory + x);

        string kirbySpritePath = "Sprites/Kirby/";

        Player.playerSprites[0] = Content.Load<Texture2D>(kirbySpritePath + "idle");
        Player.playerSprites[1] = Content.Load<Texture2D>(kirbySpritePath + "crouch");
        Player.playerSprites[2] = Content.Load<Texture2D>(kirbySpritePath + "fall");
        Player.playerSprites[3] = Content.Load<Texture2D>(kirbySpritePath + "fallFromHigh");
        Player.playerSprites[4] = Content.Load<Texture2D>(kirbySpritePath + "walk1");
        Player.playerSprites[5] = Content.Load<Texture2D>(kirbySpritePath + "walk2");
        Player.playerSprites[6] = Content.Load<Texture2D>(kirbySpritePath + "walk3");
        Player.playerSprites[7] = Player.playerSprites[5];
        Player.playerSprites[8] = Content.Load<Texture2D>(kirbySpritePath + "succStartup");
        Player.playerSprites[9] = Content.Load<Texture2D>(kirbySpritePath + "succ");
        Player.playerSprites[10] = Content.Load<Texture2D>(kirbySpritePath + "flyOpen1");
        Player.playerSprites[11] = Content.Load<Texture2D>(kirbySpritePath + "flyOpen2");
        Player.playerSprites[12] = Content.Load<Texture2D>(kirbySpritePath + "fly1");
        Player.playerSprites[13] = Content.Load<Texture2D>(kirbySpritePath + "fly2");
        Player.playerSprites[14] = Content.Load<Texture2D>(kirbySpritePath + "succIn1");
        Player.playerSprites[15] = Content.Load<Texture2D>(kirbySpritePath + "succIn2");
        Player.playerSprites[16] = Content.Load<Texture2D>(kirbySpritePath + "succJump");
        Player.playerSprites[17] = Content.Load<Texture2D>(kirbySpritePath + "succWalk1");
        Player.playerSprites[18] = Content.Load<Texture2D>(kirbySpritePath + "succWalk2");
        Player.playerSprites[19] = Content.Load<Texture2D>(kirbySpritePath + "spit1");

        Enemy1.waddleDeeSprite1 = Content.Load<Texture2D>("Sprites/Enemies/waddleDee1");
        Enemy1.waddleDeeSprite2 = Content.Load<Texture2D>("Sprites/Enemies/waddleDee2");
        Enemy2.sprite1 = Enemy1.waddleDeeSprite1;
        Enemy2.sprite2 = Enemy1.waddleDeeSprite2;
        Enemy3.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/twizzy2");
        Enemy3.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/twizzy3");
        Enemy4.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/twizzy1");
        Enemy4.sprite2 = Enemy3.sprite1;
        Enemy4.sprite3 = Enemy3.sprite2;
        Enemy5.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/brontoBurt1");
        Enemy5.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/brontoBurt2");
        Enemy7.sprite1 = Enemy5.sprite1;
        Enemy7.sprite2 = Enemy5.sprite2;
        Enemy8.sprite1 = Enemy1.waddleDeeSprite1;
        Enemy8.sprite2 = Enemy1.waddleDeeSprite2;
        Enemy9.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/poppoBrosJr1");
        Enemy9.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/poppoBrosJr2");

        for (int i = 0; i < 9; i++)
            UI.numbers[i] = Content.Load<Texture2D>($"UI/Numbers/{i}");
        UI.baseSprite = Content.Load<Texture2D>("UI/UI");
        UI.lifeEmpty = Content.Load<Texture2D>("UI/lifeEmpty");
        UI.lifeFull = Content.Load<Texture2D>("UI/lifeFull");

        Star.sprites[0] = Content.Load<Texture2D>("Sprites/Effects/star1");
        Star.sprites[1] = Content.Load<Texture2D>("Sprites/Effects/star2");
        Star.sprites[2] = Content.Load<Texture2D>("Sprites/Effects/star3");
        Star.sprites[3] = Content.Load<Texture2D>("Sprites/Effects/star4");

        AirPuff.sprite = Content.Load<Texture2D>("Sprites/Effects/puff");

        Player.soundEffect[0] = Content.Load<SoundEffect>("Sounds/Effects/bossDefeated");
        Player.soundEffect[1] = Content.Load<SoundEffect>("Sounds/Effects/enemyHit");
        Player.soundEffect[2] = Content.Load<SoundEffect>("Sounds/Effects/enterRoom");
        Player.soundEffect[3] = Content.Load<SoundEffect>("Sounds/Effects/explosion");
        Player.soundEffect[4] = Content.Load<SoundEffect>("Sounds/Effects/grabFinishStar");
        Player.soundEffect[5] = Content.Load<SoundEffect>("Sounds/Effects/healthUpPerBar");
        Player.soundEffect[6] = Content.Load<SoundEffect>("Sounds/Effects/healWhenFull");
        Player.soundEffect[7] = Content.Load<SoundEffect>("Sounds/Effects/jump");
        Player.soundEffect[8] = Content.Load<SoundEffect>("Sounds/Effects/land");
        Player.soundEffect[9] = Content.Load<SoundEffect>("Sounds/Effects/landFromHigh");
        Player.soundEffect[10] = Content.Load<SoundEffect>("Sounds/Effects/miniBossDefeated");
        Player.soundEffect[11] = Content.Load<SoundEffect>("Sounds/Effects/playerHit");
        Player.soundEffect[12] = Content.Load<SoundEffect>("Sounds/Effects/puffOut");
        Player.soundEffect[13] = Content.Load<SoundEffect>("Sounds/Effects/spit");
        Player.soundEffect[14] = Content.Load<SoundEffect>("Sounds/Effects/succ");
        Player.soundEffect[15] = Content.Load<SoundEffect>("Sounds/Effects/succIn");
        Player.soundEffect[16] = Content.Load<SoundEffect>("Sounds/Effects/swallow");
        Player.soundEffect[17] = Content.Load<SoundEffect>("Sounds/Effects/warpStar");

        level.greenGreens = Content.Load<Song>("Sounds/Music/greenGreens");
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

        spriteBatch.Begin(0, null, SamplerState.PointClamp);

        switch (GameState)
        {
            case GameStates.TitleScreen:
                spriteBatch.Draw(titleScreen, Vector2.Zero, null, Color.White, 0, Vector2.Zero, SpriteScale, 0, 0);
                break;
            case GameStates.Playing:
                level.Draw(spriteBatch, gameTime);
                break;
        }

        spriteBatch.End();
    }

}
