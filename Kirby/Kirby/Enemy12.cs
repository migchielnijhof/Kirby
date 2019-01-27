using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy12 : Enemy
{

    protected const float movementSpeed = 0.22f * Game.SpriteScale;

    public static Texture2D sprite1;

    public static Texture2D sprite2;

    protected byte animationTimer;

    protected byte WalkTimer;

    public Enemy12(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        WalkTimer = 8;
        Gravity = 0.04f * Game.SpriteScale;
        spriteSizeOffset.X = -4;
        spriteSizeOffset.Y = -8;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (!beingSucked)
            {
                if (onGround)
                {
                    if (WalkTimer == 8)
                    {
                        WalkTimer = 0;
                        if (Velocity.X > 0)
                        {
                            Velocity.X = -movementSpeed;
                            spriteEffect = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            Velocity.X = movementSpeed;
                            spriteEffect = SpriteEffects.None;
                        }
                    }
                    currentSprite = sprite2;
                    Velocity.Y = -3;
                    WalkTimer++;
                    animationTimer = 0;
                }
                else
                {
                    animationTimer++;
                    if (animationTimer == 4)
                    {
                        currentSprite = sprite1;
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