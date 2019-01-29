using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Enemy11 : Enemy
{

    protected const float movementSpeed = 0.4f * Game.SpriteScale;

    public static Texture2D sprite1;
    public static Texture2D sprite2;
    public static Texture2D sprite3;
    public static Texture2D sprite4;
    public static Texture2D asprite1;
    public static Texture2D asprite2;
    public static Texture2D asprite3;
    public static Texture2D asprite4;

    byte timer;

    byte animationTimer;

    public bool apple;

    public Enemy11(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        succResistance = true;
        spriteSizeOffset.Y = -16;
        animationTimer = 1;
        Velocity.X = movementSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (!beingSucked)
            {
                if (onGround && apple)
                {
                    succResistance = false;
                    Random random = new Random();
                    int spriteRandom = random.Next(4);
                    currentSprite = asprite1;//[spriteRandom + 1];
                    if (Velocity.X == -4 || Velocity.X == 4)
                    {
                        Velocity.X /= 2;
                        Velocity.Y = -1;
                    }
                    else
                        Velocity.X = 0;
                }
                else
                {
                    timer++;
                    if (timer == 10 && !apple)
                    {
                        timer = 0;
                        if (Velocity.X > 0)
                            animationTimer++;
                        else
                            animationTimer--;
                        switch (animationTimer)
                        {
                            case 1:
                                currentSprite = sprite1;
                                Velocity.X = movementSpeed;
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
                            case 5:
                                currentSprite = sprite1;
                                break;
                            case 6:
                                currentSprite = sprite2;
                                break;
                            case 7:
                                currentSprite = sprite3;
                                break;
                            case 8:
                                currentSprite = sprite4;
                                Velocity.X = -movementSpeed;
                                break;
                        }
                    }
                }

            }
            else
            {
                if (!apple)
                {
                    apple = true;
                    Enemy e = new Enemy9(parent as Level);
                    e.Position = Position;
                    (parent as Level).Add(e);
                    Player p = (parent as Level).Find(ObjectType.Player) as Player;
                    spriteSizeOffset.Y = 0;
                    currentSprite = asprite2;
                    if (p.Position.X < Position.X)
                    {
                        Velocity.Y = -4;
                        Velocity.X = 4;
                    }
                    else
                    {
                        Velocity.Y = -4;
                        Velocity.X = -4;
                    }
                }
            }
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}