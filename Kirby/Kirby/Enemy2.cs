using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy2 : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public static Texture2D sprite1;

    public static Texture2D sprite2;

    protected byte WalkTimer;

    protected byte animationTimer;

    public Enemy2(GameObject parent) : base (parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        WalkTimer = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (animationTimer < 16)
                animationTimer++;
            else
            {
                animationTimer = 0;
            }
            if (!beingSucked)
            {
                if (WalkTimer < 53)
                {
                    Velocity.X = -movementSpeed;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    Velocity.X = movementSpeed;
                    spriteEffect = SpriteEffects.None;
                }
                WalkTimer++;
                if (WalkTimer == 105)
                    WalkTimer = 0;
                if (animationTimer == 0)
                {
                    if (currentSprite == sprite2)
                        currentSprite = sprite1;
                    else
                        currentSprite = sprite2;
                }
            }
            else
                currentSprite = sprite2;
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}