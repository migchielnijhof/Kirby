using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy1 : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public static Texture2D waddleDeeSprite1;

    public static Texture2D waddleDeeSprite2;

    protected byte animationTimer;

    public Enemy1(GameObject parent) : base (parent, 200, 400, 400, 400)
    {
        currentSprite = waddleDeeSprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
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
                Velocity.X = -movementSpeed;
                if (animationTimer == 0)
                {
                    if (currentSprite == waddleDeeSprite2)
                        currentSprite = waddleDeeSprite1;
                    else
                        currentSprite = waddleDeeSprite2;
                }
            }
            else
                currentSprite = waddleDeeSprite2;
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}