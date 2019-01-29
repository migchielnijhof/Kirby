using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Enemy15 : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;
    
    public static Texture2D sprite2;
    public static Texture2D sprite3;

    public Enemy15(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite2;
        spriteEffect = SpriteEffects.FlipHorizontally;
        succResistance = false;
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
                        int spriteRandom = random.Next(2);
                        if (spriteRandom == 1)
                            currentSprite = sprite2;
                        else
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