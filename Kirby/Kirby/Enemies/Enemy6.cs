using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Enemy6 : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public static Texture2D sprite1;
    public static Texture2D sprite2;
    public static Texture2D sprite3;
    public bool panicing;

    public Enemy6(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
        succResistance = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (!beingSucked)
            {
                if (onGround)
                {
                    Velocity.Y = -4;
                    Random random = new Random();
                    int direction = random.Next(2);
                    if (direction == 1)
                        Velocity.X = 0.75f;
                    else
                        Velocity.X = -0.75f;
                    int flipRandom = random.Next(2);
                    if (flipRandom == 1)
                        spriteEffect = SpriteEffects.FlipHorizontally;
                    else
                        spriteEffect = SpriteEffects.None;
                    if (panicing)
                    {
                        succResistance = false;
                        int spriteRandom = random.Next(2);
                        if (spriteRandom == 1)
                            currentSprite = sprite2;
                        else
                            currentSprite = sprite3;
                    }
                }

            }
            else
            {
                if (!panicing)
                {
                    panicing = true;
                    Enemy e = new Enemy6Cap(parent as Level);
                    e.Position = Position;
                    (parent as Level).Add(e);
                    Player p = (parent as Level).Find(ObjectType.Player) as Player;
                    if (p.Position.X < Position.X)
                    {
                        Velocity.Y = -5;
                        Velocity.X = 5;
                    }
                    else
                    {
                        Velocity.Y = -5;
                        Velocity.X = -5;
                    }
                    currentSprite = sprite3;
                }
            }
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}