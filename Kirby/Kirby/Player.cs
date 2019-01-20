using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// Class of the playable character.
/// </summary>
class Player : AnimatedGameObject
{
    /// <summary>
    /// Array of the player's sprites.
    /// </summary>
    public static Texture2D[] playerSprites = new Texture2D[10];

    /// <summary>
    /// Current health of the player.
    /// </summary>
    public byte Health;

    /// <summary>
    /// The position of the player.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The velocity of the player.
    /// </summary>
    public Vector2 Velocity;

    /// <summary>
    /// The bounding box of the player.
    /// </summary>
    public Rectangle BoundingBox
    {
        get
        {
            boundingBox.Location = new Point((int) Position.X, (int) Position.Y);
            return boundingBox;
        }
    }
    protected Rectangle boundingBox;

    /// <summary>
    /// The X size of the player's bounding box.
    /// </summary>
    const int BoundingBoxSizeX = (int) (16 * Game.SpriteScale);

    /// <summary>
    /// The Y size of the player's bounding box.
    /// </summary>
    const int BoundingBoxSizeY = (int) (16 * Game.SpriteScale);

    /// <summary>
    /// Maximum health of the player.
    /// </summary>
    public const byte maxHealth = 6;

    /// <summary>
    /// The movement speed of a player.
    /// </summary>
    const float movementSpeed = 45 * Game.SpriteScale;

    /// <summary>
    /// The gravity of the player.
    /// </summary>
    const float gravity = 6 * Game.SpriteScale;

    /// <summary>
    /// The player's score.
    /// </summary>
    public int score;

    /// <summary>
    /// If the player is walking.
    /// </summary>
    public bool walking;

    /// <summary>
    /// If the player is crouching.
    /// </summary>
    public bool crouching;

    /// <summary>
    /// If the player is airborne because of a jump.
    /// </summary>
    public bool jumping;

    /// <summary>
    /// The player's state, used mostly for spritework. To see what number relates to what sprite, see Game.cs around line 100.
    /// </summary>
    public int playerState;

    /// <summary>
    /// Whether the player is on the ground or not.
    /// </summary>
    protected bool onGround;

    /// <summary>
    /// The timer that counts down to when the player can move again after landing.
    /// </summary>
    protected int landingTimer;

    /// <summary>
    /// The enemy the player has sucked up.
    /// </summary>
    protected GameObject absorbedEnemy;

    /// <summary>
    /// The amount of time the player has left where holding the jump button will result in a higher jump.
    /// </summary>
    protected int highJumpTimer;

    /// <summary>
    /// The amount of time the player will have where holding the jump button will result in a higher jump.
    /// </summary>
    const int highJumpFrames = 8;

    /// <summary>
    /// The amount of time the player will crouch when landing.
    /// </summary>
    const int landingLag = 7;

    /// <summary>
    /// The speed of animations
    /// </summary>
    const int animationSpeed = 8;

    /// <summary>
    /// The timer used in animation.
    /// </summary>
    protected int animationTimer;

    /// <summary>
    /// The amount of time the player will remain invulnerable.
    /// </summary>
    protected double invulnerabilityTime;

    /// <summary>
    /// The amount of invulnerabiliyTime the player will get after taking damage.
    /// </summary>
    const double invulnerability = 1.0d;

    /// <summary>
    /// The spriteeffects, used for mirroring.
    /// </summary>
    SpriteEffects s = SpriteEffects.None;

    public bool previousFrameJump;

    /// <summary>
    /// Create a new player.
    /// </summary>
    /// <param name="parent">The game object can communicate with other game objects through the parent.</param>
    public Player(GameObject parent) : base(parent, ObjectType.Player)
    {
        Health = maxHealth; //The player starts with maximum health
        score = 0; //The player starts without a score
        absorbedEnemy = null; //The player starts without an enemy absorbed
        invulnerabilityTime = 0.0d; //The player starts without invulnerability
        Velocity = Vector2.Zero; //The player starts without any velocity
        onGround = true; //The player starts on the ground
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY); //Sets the bounding box size
        playerState = 0;
        highJumpTimer = highJumpFrames;
    }

    public void TakeDamage()
    {
        if (invulnerabilityTime > 0) //The player shouldn't receive damage if they're still invulnerable
            return;
        Health--; //Gets hit
        if (Health == 0) //If you don't have any health
        {
            Die(); //You die
            return;
        }
        invulnerabilityTime = invulnerability; //gives the player invulnerability
    }

    protected void Die()
    {

    }

    public override void HandleInput(Input input)
    {
        Velocity.X = 0;

        if (input.Crouch && onGround) //Crouching
        {
            playerState = 1;
            crouching = true;
            return;
        }
        else crouching = false;

        if (input.Movement == 1) //Walking right
        {
            Velocity.X = movementSpeed;
            s = SpriteEffects.None;
            walking = true;
        }
        else if (input.Movement == 2) //Walking left
        {
            Velocity.X = -movementSpeed;
            s = SpriteEffects.FlipHorizontally;
            walking = true;
        }
        else walking = false;

        if (input.Jump) //Jumps when you press the jump key
        {
            if ((highJumpTimer < highJumpFrames && !onGround) || (onGround && !previousFrameJump)) //The user can only rise into the air if the user is either on the ground, or has just started the jump
            {
                double jumpMultiplier = (Math.Sqrt((1-(highJumpTimer / highJumpFrames))*10) - 1) * 100;
                if ((!previousFrameJump && onGround && landingTimer < (landingLag / 2)) || jumping) //If this is the first frame of the jump
                {
                    if (!jumping)
                    {
                        highJumpTimer = 1;
                        jumpMultiplier = 100;
                        jumping = true;
                    }
                    Velocity.Y = (-64 * Game.SpriteScale * (int)jumpMultiplier) / 100; //The rest of the jump is faster to not make it feel sluggish
                }
            }
            previousFrameJump = true;
        }
        else
        {
            previousFrameJump = false;
            if (Velocity.Y < 0)
            {
                Velocity.Y = 0;
            }
        }

        if (highJumpTimer < highJumpFrames) //The timer increases until it reaches its max, which determines the max height of the jump
        {
            highJumpTimer++;
        }

        if (landingTimer >= 1 && onGround) //The following code is to make sure Kirby crouches when landing, and resets some code relating to the jumop height timer
        {
            highJumpTimer = 1;
            landingTimer--;
            playerState = 1;
            jumping = false;
        }
        else if (onGround)
        {
            if (((playerState < 4 || playerState > 7) || Velocity.X == 0) && landingTimer == 0)
            {
                playerState = 0;
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (animationTimer < animationSpeed) //Manages the animation speed. Currently only used for Kirby's walk cycle.
        {
            animationTimer++;
        }
        else
        {
            animationTimer = 0;
        }

        if (!onGround) //Sends the player down if they're not on the ground
        {
            Velocity.Y += gravity;
            playerState = 2;
            landingTimer = landingLag;
        }
        else if (Velocity.Y > 0) //The player can't move on the Y axis when on the ground.
            Velocity.Y = 0;

        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (Position.X < 0) //The player can't move past the left edge of the screen
            Position.X = 0;

        if (Position.Y < 0) //The player can't move past the top edge of the screen
            Position.Y = 0;

        TileGrid grid = (parent as Level).Find(ObjectType.TileGrid) as TileGrid;

        try //Collision with the tilegrid
        {
            TileCollision(grid, 0, 0);
            TileCollision(grid, 1, 0);
            TileCollision(grid, 0, 1);
            TileCollision(grid, 1, 1);
            if (TestGround(grid))
            {
                onGround = true;
            }
            else
                onGround = false;
        }
        catch

        {
            TakeDamage();
        }
        
        if (walking && onGround && animationTimer == 0 && landingTimer == 0 && !crouching) //Plays the walking animation
        {
            switch (playerState)
            {
                case 4:
                case 5:
                case 6:
                    playerState++;
                    break;

                default:
                    playerState = 4;
                    break;
            }
        }
    }

    public bool TestGround(TileGrid grid)
    {
        byte x = grid.GetIndexX(Position.X);
        byte y = grid.GetIndexY(Position.Y + BoundingBox.Size.Y);
        return grid.tiles[x, y].Solid || grid.tiles[x + 1, y].Solid;
    }

    public void TileCollision(TileGrid grid, byte offsetX, byte offsetY)
    {
        byte x = (byte) (grid.GetIndexX(Position.X) + offsetX);
        byte y = (byte) (grid.GetIndexY(Position.Y) + offsetY);
        Rectangle tile = grid.GetBoundingBox(x, y);
        if (grid.tiles[x, y].Solid && (BoundingBox.Intersects(tile) || BoundingBox.Contains(tile) || tile.Contains(BoundingBox)))
        {
            Console.WriteLine("i");
            int a = BoundingBox.Center.X - tile.Center.X;
            int b = BoundingBox.Center.Y - tile.Center.Y;
            if (Math.Abs(a) > Math.Abs(b))
            {
                if (a > 0)
                    Position.X = tile.X + tile.Size.X;
                else
                    Position.X = tile.X - BoundingBox.Size.X;
                Velocity.X = 0;
            }
            else
            {
                if (b > 0)
                    Position.Y = tile.Y + tile.Size.Y;
                else
                    Position.Y = tile.Y - BoundingBox.Size.Y;
                Velocity.Y = 0;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(playerSprites[playerState], Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, s, 0);
    }

}