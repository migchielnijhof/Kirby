using Microsoft.Xna.Framework;
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
    public const byte basePlayerLives = 4;

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

        Block.sprite = Content.Load<Texture2D>("Sprites/Effects/block");

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
        Enemy6.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/cappyHidden");
        Enemy6.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/cappyPanic1");
        Enemy6.sprite3 = Content.Load<Texture2D>("Sprites/Enemies/cappyPanic2");
        Enemy6Cap.sprite = Content.Load<Texture2D>("Sprites/Enemies/cappyCap");
        Enemy7.sprite1 = Enemy5.sprite1;
        Enemy7.sprite2 = Enemy5.sprite2;
        Enemy8.sprite1 = Enemy1.waddleDeeSprite1;
        Enemy8.sprite2 = Enemy1.waddleDeeSprite2;
        Enemy9.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/poppoBrosJr1");
        Enemy9.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/poppoBrosJr2");
        Enemy11.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/a1");
        Enemy11.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/a2");
        Enemy11.sprite3 = Content.Load<Texture2D>("Sprites/Enemies/a3");
        Enemy11.sprite4 = Content.Load<Texture2D>("Sprites/Enemies/a4");
        Enemy11.asprite1 = Content.Load<Texture2D>("Sprites/Enemies/apple1");
        Enemy11.asprite2 = Content.Load<Texture2D>("Sprites/Enemies/apple2");
        Enemy11.asprite3 = Content.Load<Texture2D>("Sprites/Enemies/apple3");
        Enemy11.asprite4 = Content.Load<Texture2D>("Sprites/Enemies/apple4");
        Enemy12.sprite1 = Content.Load<Texture2D>("Sprites/Enemies/grizzo1");
        Enemy12.sprite2 = Content.Load<Texture2D>("Sprites/Enemies/grizzo2");
        Enemy14.sprite1 = Enemy12.sprite1;
        Enemy14.sprite2 = Enemy12.sprite2;
        Enemy15.sprite3 = Enemy6.sprite3;
        Enemy15.sprite2 = Enemy6.sprite2;

        PopoBrosSr.sprite1 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/IdleJump1");
        PopoBrosSr.sprite2 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/IdleJump2");
        PopoBrosSr.sprite3 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/IdleJump3");
        PopoBrosSr.sprite4 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/IdleJump4");
        PopoBrosSr.throwSprite1 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombThrow1");
        PopoBrosSr.throwSprite2 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombThrow2");
        PopoBrosSr.throwSprite3 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombThrow3");
        PopoBrosSr.throwSprite4 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombThrow4");
        PopoBrosSr.throwSprite5 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombThrow5");
        PopoBrosSr.windUpSprite1 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombThrow5");
        PopoBrosSr.windUpSprite2 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombWindUp1");
        PopoBrosSr.windUpSprite3 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombWindUp2");
        PopoBrosSr.windUpSprite4 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombWindUp3");
        PopoBrosSr.windUpSprite5 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombWindUp4");
        PopoBrosSr.windUpSprite6 = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bombWindUp5");

        WhispyWoods.sprite1 = Content.Load<Texture2D>("Sprites/Bosses/WhispyWoods/sprite1");
        WhispyWoods.sprite2 = Content.Load<Texture2D>("Sprites/Bosses/WhispyWoods/sprite2");
        WhispyWoods.sprite3 = Content.Load<Texture2D>("Sprites/Bosses/WhispyWoods/sprite3");
        WhispyWoods.sprite4 = Content.Load<Texture2D>("Sprites/Bosses/WhispyWoods/sprite4");

        BossPuff.sprite1 = Content.Load<Texture2D>("Sprites/Effects/puff");

        Apple.sprite = Enemy11.asprite1;

        BossHitBox.sprite = Content.Load<Texture2D>("Sprites/Bosses/WhispyWoods/hitbox");

        Bomb.sprite = Content.Load<Texture2D>("Sprites/Bosses/popoBrosSr/bomb");

        for (int i = 0; i <= 9; i++)
            UI.numbers[i] = Content.Load<Texture2D>($"UI/Numbers/{i}");
        UI.baseSprite = Content.Load<Texture2D>("UI/UI");
        UI.bossSprite = Content.Load<Texture2D>("UI/bossUI");
        UI.lifeEmpty = Content.Load<Texture2D>("UI/lifeEmpty");
        UI.lifeFull = Content.Load<Texture2D>("UI/lifeFull");
        UI.bossHealth = Content.Load<Texture2D>("UI/bossHealth");

        Star.sprites[0] = Content.Load<Texture2D>("Sprites/Effects/star1");
        Star.sprites[1] = Content.Load<Texture2D>("Sprites/Effects/star2");
        Star.sprites[2] = Content.Load<Texture2D>("Sprites/Effects/star3");
        Star.sprites[3] = Content.Load<Texture2D>("Sprites/Effects/star4");

        WarpStar.sprites[1] = Star.sprites[0];
        WarpStar.sprites[2] = Star.sprites[1];
        WarpStar.sprites[3] = Star.sprites[2];
        WarpStar.sprites[4] = Star.sprites[3];

        SuccParticle.sprite = Content.Load<Texture2D>("Sprites/Effects/succParticle");

        EnemyDefeatParticle.sprites[0] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion1");
        EnemyDefeatParticle.sprites[1] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion2");
        EnemyDefeatParticle.sprites[2] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion3");
        EnemyDefeatParticle.sprites[3] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion4");
        EnemyDefeatParticle.sprites[4] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion5");
        EnemyDefeatParticle.sprites[5] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion6");
        EnemyDefeatParticle.sprites[6] = Content.Load<Texture2D>("Sprites/ElaborateEffects/spitExplosion7");

        for (int i = 1; i <= 19; i++)
            BossDefeatParticle.sprites[i] = Content.Load<Texture2D>($"Sprites/ElaborateEffects/bossDefeat-{i}");

        AirPuff.sprite = BossPuff.sprite1;

        HealingItem.sprite = Content.Load<Texture2D>("Sprites/Effects/healingItem");

        MaximTomato.sprite = Content.Load<Texture2D>("Sprites/Effects/maximTomato");

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

        Enemy.hitEffect = Content.Load<SoundEffect>("Sounds/Effects/enemyHit");

        Boss.boossDefeatEffect = Content.Load<SoundEffect>("Sounds/Effects/bossDefeated");

        WhispyWoods.puffSound = Player.soundEffect[12];

        level.greenGreens = Content.Load<Song>("Sounds/Music/greenGreens");
        level.bossTheme = Content.Load<Song>("Sounds/Music/bossTheme");
        level.sparklingStars = Content.Load<Song>("Sounds/Music/sparklingStars");
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
                    level.Load(true, 1, new Player(level));
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
