using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

class PopoBrosSr : Boss
{

    protected const float movementSpeed = 0.4f * Game.SpriteScale;

    protected int timer;

    protected int timer2;

    protected int attackTimer;

    protected byte spriteState;

    protected byte attackState;

    protected byte otherAttackChance = 0;

    public static Texture2D sprite1;
    public static Texture2D sprite2;
    public static Texture2D sprite3;
    public static Texture2D sprite4;

    public static Texture2D throwSprite1;
    public static Texture2D throwSprite2;
    public static Texture2D throwSprite3;
    public static Texture2D throwSprite4;
    public static Texture2D throwSprite5;

    public static Texture2D windUpSprite1;
    public static Texture2D windUpSprite2;
    public static Texture2D windUpSprite3;
    public static Texture2D windUpSprite4;
    public static Texture2D windUpSprite5;
    public static Texture2D windUpSprite6;

    public PopoBrosSr(GameObject parent) : base(parent, 200, 3000, 400, 400)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        currentSprite = sprite1;
        spriteSizeOffset.Y = -28;
        spriteSizeOffset.X = -19;
        Gravity = 0.5f;
        loaded = true;
        timer = 0;
        timer2 = 0;
        spriteState = 1;
        attackState = 1;
        attackTimer = 0;
        p.level.bossHealth = 3;

        MediaPlayer.Stop();
        MediaPlayer.Play((parent as Level).bossTheme);

        p.level.cameraLocked = true;
    }

    public override void Update(GameTime gameTime)
    {
        beingSucked = false;
        if (alive && loaded)
        {
            switch (attackState)
            {
                case 1:
                    if (onGround)
                    {
                        if (attackTimer == 4)
                        {
                            Random random = new Random();
                            int r = random.Next(5);
                            if (r + 1 <= otherAttackChance)
                            {
                                attackState = 3;
                                otherAttackChance = 0;
                            }
                            else
                            {
                                attackState = 2;
                                otherAttackChance++;
                            }
                                timer = 0;
                        }
                        else
                        {
                            if (Velocity.X == 0)
                                Velocity.X = movementSpeed;
                            else
                                Velocity.X *= -1;
                            Velocity.Y = -8;
                            timer = 1;
                            attackTimer++;
                        }
                    }
                    if (timer > 0)
                    {
                        timer++;
                    }
                    if (timer == 8)
                    {
                        timer = 1;
                        if (Velocity.X > 0 && spriteState != 4)
                            spriteState++;
                        else if (Velocity.X < 0 && spriteState != 1)
                            spriteState--;
                    }
                    switch (spriteState)
                    {
                        case 1:
                            currentSprite = sprite1;
                            break;
                        case 2:
                            currentSprite = sprite2;
                            break;
                        case 3:
                            currentSprite = sprite3;
                            break;
                        case 4:
                            currentSprite = sprite4;
                            break;
                    }
                    break;
                case 2:
                    switch (timer)
                    {
                        case 1:
                            Velocity.X = 0;
                            Velocity.Y = -15;
                            currentSprite = windUpSprite1;
                            break;
                        case 8:
                            currentSprite = windUpSprite2;
                            break;
                        case 14:
                            currentSprite = windUpSprite3;
                            break;
                        case 20:
                            currentSprite = windUpSprite4;
                            break;
                        case 26:
                            currentSprite = windUpSprite5;
                            break;
                        case 32:
                            currentSprite = windUpSprite6;
                            Gravity = 0;
                            Random random = new Random();
                            int r = random.Next(2);
                            if (r == 1)
                                timer = 42;
                            break;
                        case 52:
                            Gravity = 0.5f;
                            break;
                        case 72:
                            currentSprite = throwSprite2;
                            break;
                        case 74:
                            currentSprite = throwSprite3;
                            break;
                        case 78:
                            currentSprite = throwSprite4;
                            break;
                        case 80:
                            currentSprite = throwSprite5;
                            Enemy e = new Bomb(parent as Level);
                            e.Position.X = Position.X - 32;
                            e.Position.Y = Position.Y;
                            (parent as Level).Add(e);
                            break;
                        case 88:
                            attackState = 1;
                            attackTimer = 0;
                            timer = 0;
                            break;
                    }
                    timer++;
                    break;
                case 3:
                    if (onGround)
                    {
                        timer++;
                        switch (timer)
                        {
                            case 1:
                                Velocity.X = -2.5f * movementSpeed;
                                Velocity.Y = -10;
                                break;
                            case 2:
                                Velocity.X *= -1;
                                Velocity.Y = -10;
                                break;
                            case 3:
                                Velocity.X = -6 * movementSpeed;
                                Velocity.Y = -12;
                                break;
                            case 4:
                                Velocity.X *= -1;
                                Velocity.Y = -12;
                                break;
                            case 5:
                                attackState = 1;
                                attackTimer = 0;
                                Velocity.X = 0;
                                timer = 0;
                                break;
                        }
                    }
                    if (timer2 == 16)
                    {
                        timer2 = 1;
                        if (Velocity.X > 0 && spriteState != 4)
                            spriteState++;
                        else if (Velocity.X < 0 && spriteState != 1)
                            spriteState--;
                    }
                    switch (spriteState)
                    {
                        case 1:
                            currentSprite = sprite1;
                            break;
                        case 2:
                            currentSprite = sprite2;
                            break;
                        case 3:
                            currentSprite = sprite3;
                            break;
                        case 4:
                            currentSprite = sprite4;
                            break;
                    }
                    timer2++;
                    break;
            }
            DoPhysics();
            beingSucked = false;
        }
        
    }

}