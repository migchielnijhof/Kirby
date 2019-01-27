﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

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

    public static SoundEffect[] soundEffect = new SoundEffect[18];

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
    /// If the player has an enemy absorbed.
    /// </summary>
    protected bool fat;

    /// <summary>
    /// The enemy the player has sucked up.
    /// </summary>
    public GameObject absorbedEnemy;

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
    /// The timer used in the walking animation.
    /// </summary>
    protected int walkAnimationTimer;

    /// <summary>
    /// The timer used in the sucking animation.
    /// </summary>
    protected byte succAnimationTimer;

    /// <summary>
    /// The timer used in the spitting animation.
    /// </summary>
    protected byte spitAnimationTimer;

    /// <summary>
    /// The timer used in the flying animation.
    /// </summary>
    protected byte flyAnimationTimer;

    /// <summary>
    /// The timer used in the flying animation.
    /// </summary>
    protected byte framesToNextFlyStage;

    /// <summary>
    /// Keeps track of the current stage of Kirby's flight.
    /// </summary>
    protected byte flyStage;

    /// <summary>
    /// The amount of time the player will remain invulnerable.
    /// </summary>
    protected double invulnerabilityTime;

    /// <summary>
    /// Offset used when Kirby grows in size so that he remains in the same position.
    /// </summary>
    protected Vector2 spriteSizeOffset;

    /// <summary>
    /// The amount of invulnerabiliyTime the player will get after taking damage.
    /// </summary>
    const double invulnerability = 0.5d;

    /// <summary>
    /// The spriteeffects, used for mirroring.
    /// </summary>
    SpriteEffects s;

    public bool previousFrameJump;

    protected const float SuckAcceleration = 0.3f * Game.SpriteScale;

    protected const int SuckX = (int)(32 * Game.SpriteScale);
    protected const int SuckY = (int)(32 * Game.SpriteScale);

    protected bool previousSuck;

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
        s = SpriteEffects.None;
        flying = false;
        flyStage = 0;
        highJumpTimer = highJumpFrames;
    }

    public void TakeDamage()
    {
        if (invulnerabilityTime > 0 | Health == 0) //The player shouldn't receive damage if they're invulnerable or don't have any health
            return;
        Health--; //Gets hit
        soundEffect[11].Play();
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
            if (fat)
            {
                Jump();
            }
            else
            {
                Fly();
            }
        }

        if (input.Crouch && onGround && !flying && !fat) //Crouching
        {
            playerState = 1;
            crouching = true;
            return;
        }
        else crouching = false;

        if (spitAnimationTimer == 0)
        {
            if (input.Succ)
            {
                walking = false;
                bool mirrored = (s == SpriteEffects.FlipHorizontally);
                if (!previousSuck && absorbedEnemy != null)
                {
                    soundEffect[13].Play();
                    Star s = new Star(parent);
                    if (!mirrored)
                    {
                        s.Position = new Vector2(BoundingBox.Right, BoundingBox.Center.Y - Star.BoundingBoxY / 2);
                        s.Velocity.X = Star.Speed;
                    }
                    else
                    {
                        s.Position = new Vector2(BoundingBox.Left - Star.BoundingBoxX, BoundingBox.Center.Y - Star.BoundingBoxY / 2);
                        s.Velocity.X = -Star.Speed;
                    }
                    (parent as Level).Add(s);
                    absorbedEnemy = null;
                    spitAnimationTimer = 1;
                    return;
                }
                if (flying && flyStage < 7)
                {
                    AirPuff p = new AirPuff(parent);
                    if (!mirrored)
                    {
                        p.Position = new Vector2(BoundingBox.Right, BoundingBox.Center.Y - AirPuff.BoundingBoxY / 2);
                        p.Velocity.X = AirPuff.Speed;
                    }
                    else
                    {
                        p.Position = new Vector2(BoundingBox.Left - AirPuff.BoundingBoxX, BoundingBox.Center.Y - AirPuff.BoundingBoxY / 2);
                        p.Velocity.X = -AirPuff.Speed;
                    }
                    (parent as Level).Add(p);
                    flyStage = 7;
                    soundEffect[12].Play();
                    return;
                }
                Rectangle succBox;
                if (!mirrored)
                    succBox = new Rectangle(new Point(BoundingBox.Right, BoundingBox.Top), new Point(SuckX, SuckY));
                else
                    succBox = new Rectangle(new Point(BoundingBox.Left - SuckX, BoundingBox.Top), new Point(SuckX, SuckY));

                List<GameObject> enemies = (parent as Level).FindAll(ObjectType.Enemy) as List<GameObject>;
                foreach (Enemy e in enemies)
                    if (e.BoundingBox.Intersects(succBox) || succBox.Contains(e.BoundingBox))
                    {
                        e.beingSucked = true;
                        if (!mirrored)
                        {
                            e.Velocity.X -= SuckAcceleration;
                            continue;
                        }
                        e.Velocity.X += SuckAcceleration;
                    }
                previousSuck = true;

                //if (succAnimationTimer == 1)
                {
                //    soundEffect[14].Play();
                }

                if (succAnimationTimer < 4)
                {
                    playerState = 8;
                    succAnimationTimer++;
                }
                else
                    playerState = 9;

                if (playerState == 9 && absorbedEnemy != null)
                    playerState = 15;

                return;
            }
            else
            {
                previousSuck = false;
                succAnimationTimer = 1;
            }
        }
        else
        {
            switch (spitAnimationTimer)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    playerState = 19;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    playerState = 9;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    playerState = 8;
                    break;
                case 18:
                    spitAnimationTimer = 0;
                    break;
            }
            if (spitAnimationTimer != 0)
                spitAnimationTimer++;
            return;
        }
        
        if (absorbedEnemy != null)
        {
            if (fat == false)
                soundEffect[15].Play();
            fat = true;
        }
        else
        {
            fat = false;
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
            if (flying)
                Fly();
            else
                Jump();
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
            if (!fat)
            {
                playerState = 1;
            }
            jumping = false;
            if (landingTimer == landingLag - 1)
            {
                soundEffect[8].Play();
            }
        }
        else if ((((playerState < 4 || playerState > 7) && playerState != 17 && playerState != 18) || Velocity.X == 0) && landingTimer == 0 && onGround && !input.Succ) //If the player isn't walking while on the ground, crouching, or landing, they will simply stand still.
        {
            if (!fat)
                playerState = 0;
            else
                playerState = 15;
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
                    soundEffect[7].Play();
                }
                Velocity.Y = (-64 * Game.SpriteScale * (int)jumpMultiplier) / 6000;
            }
        }
        previousFrameJump = true;
    }

    public void Fly()
    {
        if (!flying)
            flyStage = 1;
        flying = true;
        flyingUp = true;
        Velocity.Y = (-3 * Game.SpriteScale)/2;
    }

    public override void Update(GameTime gameTime)
    {
        if (invulnerabilityTime > 0)
                invulnerabilityTime -= gameTime.ElapsedGameTime.TotalSeconds;

        DoPhysics();

        if (walkAnimationTimer < animationSpeed) //Manages the animation speed. Currently only used for Kirby's walk cycle.
        {
            walkAnimationTimer++;
        }
        else
        {
            walkAnimationTimer = 0;
        }

        if (!onGround)
        {
            if (!flying && succAnimationTimer == 1)
            {
                Velocity.Y += Gravity;
                if (fat)
                {
                    playerState = 16;
                }
                else
                    playerState = 2;
                landingTimer = landingLag;
            }
            else if (!flyingUp && Velocity.Y >= (Gravity * Game.SpriteScale) * 3)
            {
                Velocity.Y = (Gravity * Game.SpriteScale) * 3;
            }
        }

        if (walking && onGround && walkAnimationTimer == 0 && landingTimer == 0 && !crouching) //Plays the walking animation
        {
            if (fat)
            {
                if (playerState == 17)
                    playerState = 18;
                else
                    playerState = 17;
            }
            else
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

        if (flying)
        {
            switch (flyStage)
            {
                case 1:
                    playerState = 8;
                    framesToNextFlyStage = 10;
                    break;
                case 2:
                    playerState = 9;
                    break;
                case 3:
                    playerState = 10;
                    break;
                case 4:
                    playerState = 11;
                    framesToNextFlyStage = 1;
                    break;
                case 5:
                    playerState = 12;
                    framesToNextFlyStage = 15;
                    break;
                case 6:
                    playerState = 13;
                    break;
                case 7:
                    playerState = 10;
                    framesToNextFlyStage = 10;
                    break;
                case 8:
                    playerState = 11;
                    break;
                case 9:
                    playerState = 8;
                    break;
            }

            if (flyAnimationTimer >= framesToNextFlyStage)
            {
                flyAnimationTimer = 0;
                if (flyStage == 6)
                {
                    flyStage--;
                }
                else
                    flyStage++;
            }

            flyAnimationTimer++;
        }

        if (flyStage == 10)
        {
            flying = false;
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