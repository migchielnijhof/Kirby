using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// Class of the playable character.
/// </summary>
class Player : PhysicsObject
{
    /// <summary>
    /// Array of the player's sprites.
    /// </summary>
    public static Texture2D[] playerSprites = new Texture2D[20];

    /// <summary>
    /// Current health of the player.
    /// </summary>
    public byte Health;

    /// <summary>
    /// The X size of the player's bounding box.
    /// </summary>
    const int BoundingBoxSizeX = (int)(16 * Game.SpriteScale);

    /// <summary>
    /// The Y size of the player's bounding box.
    /// </summary>
    const int BoundingBoxSizeY = (int)(16 * Game.SpriteScale);

    /// <summary>
    /// Maximum health of the player.
    /// </summary>
    public const byte maxHealth = 6;

    /// <summary>
    /// The movement speed of the player.
    /// </summary>
    const float movementSpeed = 0.75f * Game.SpriteScale;

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
    /// The timer that counts down to when the player can move again after landing.
    /// </summary>
    protected int landingTimer;

    /// <summary>
    /// If the player is flying.
    /// </summary>
    protected bool flying;

    /// <summary>
    /// If the player is flying up.
    /// </summary>
    protected bool flyingUp;

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

    protected Vector2 spriteSizeOffset;

    /// <summary>
    /// The amount of invulnerabiliyTime the player will get after taking damage.
    /// </summary>
    const double invulnerability = 0.5d;

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
        flying = false;
        highJumpTimer = highJumpFrames;
    }

    public void TakeDamage()
    {
        if (invulnerabilityTime > 0 | Health == 0) //The player shouldn't receive damage if they're invulnerable or don't have any health
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
        if (Health == 0)
            return;

        Velocity.X = 0;

        if (input.Fly)
        {
            Fly();
        }

        if (input.Crouch && onGround && !flying) //Crouching
        {
            playerState = 1;
            crouching = true;
            return;
        }
        else crouching = false;

        if (input.Succ)
        {
            if (flying)
            {
                flying = false;
            }
        }

        if (input.Movement == 1) //Walking right
        {
            Velocity.X = movementSpeed;
            s = SpriteEffects.None; //Makes the player face right
            walking = true;
        }
        else if (input.Movement == 2) //Walking left
        {
            Velocity.X = -movementSpeed;
            s = SpriteEffects.FlipHorizontally; //Makes the player face left
            walking = true;
        }
        else walking = false;

        if (input.Jump) //Jumps when you press the jump key
        {
            if (!flying)
                Jump();
            else
                Fly();
        }
        else if (!input.Fly)
        {
            flyingUp = false;
            previousFrameJump = false;
            if (Velocity.Y < 0 && !flying) //If the player isn't holding the jump key anymore, they will stop going up immediately.
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
        else if (((playerState < 4 || playerState > 7) || Velocity.X == 0) && landingTimer == 0 && onGround) //If the player isn't walking while on the ground, crouching, or landing, they will simply stand still.
        {
            playerState = 0;
        }
    }

    public void Jump()
    {
        if ((highJumpTimer < highJumpFrames && !onGround) || (onGround && !previousFrameJump)) //The user can only rise into the air if the user is either on the ground, or has just started the jump
        {
            double jumpMultiplier = (Math.Sqrt((1 - (highJumpTimer / highJumpFrames)) * 10) - 1) * 100; //A multiplyer which determines how fast the player rises in the air. It's rather copmplicated, sorry.
            if ((!previousFrameJump && onGround && landingTimer < (landingLag / 2)) || jumping) //If this is the first frame of the jump
            {
                if (!jumping) //The first frame of the jump
                {
                    highJumpTimer = 1;
                    jumpMultiplier = 100;
                    jumping = true;
                }
                Velocity.Y = (-64 * Game.SpriteScale * (int)jumpMultiplier) / 6000; //The rest of the jump is faster to not make it feel sluggish
            }
        }
        previousFrameJump = true;
    }

    public void Fly()
    {
        if (!flying)
        {

        }
        flying = true;
        flyingUp = true;
        Velocity.Y = (-3 * Game.SpriteScale)/2;
    }

    public override void Update(GameTime gameTime)
    {
        if (invulnerabilityTime > 0)
                invulnerabilityTime -= gameTime.ElapsedGameTime.TotalSeconds;

        DoPhysics();

        if (animationTimer < animationSpeed) //Manages the animation speed. Currently only used for Kirby's walk cycle.
        {
            animationTimer++;
        }
        else
        {
            animationTimer = 0;
        }

        if (!onGround)
        {
            if (!flying)
            {
                Velocity.Y += Gravity;
                playerState = 2;
                landingTimer = landingLag;
            }
            else if (!flyingUp && Velocity.Y >= (Gravity * Game.SpriteScale) * 3)
            {
                Velocity.Y = (Gravity * Game.SpriteScale) * 3;
            }
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

        if (flying)
        {
            playerState = 12;
        }

        spriteSizeOffset.X = (playerSprites[0].Width - playerSprites[playerState].Width) / 2;
        spriteSizeOffset.Y = playerSprites[0].Height - playerSprites[playerState].Height;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (invulnerabilityTime > 0)
            spriteBatch.Draw(playerSprites[playerState], Position - (parent as Level).CameraPosition + spriteSizeOffset * Game.SpriteScale, null, new Color(255, 170, 170), 0, Vector2.Zero, Game.SpriteScale, s, 0);
        else
            spriteBatch.Draw(playerSprites[playerState], Position - (parent as Level).CameraPosition + spriteSizeOffset * Game.SpriteScale, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, s, 0);
    }

}