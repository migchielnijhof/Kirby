using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy8 : Enemy
{

    protected const float movementSpeed = 1.5f * Game.SpriteScale;

    public static Texture2D sprite1;

    public static Texture2D sprite2;

    protected byte WalkTimer;

    protected byte animationTimer;

    public Enemy8(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        WalkTimer = 69;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            animationTimer++;
            if (!beingSucked)
            {
                if (animationTimer < 16)
                    currentSprite = sprite1;
                else
                    currentSprite = sprite2;

                Velocity.X = (Velocity.X / 60) * 59;
                WalkTimer++;
                if (WalkTimer == 70)
                {
                    Velocity.X = -movementSpeed;
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    animationTimer = 0;
                }
                if (WalkTimer == 140)
                {
                    Velocity.X = movementSpeed;
                    WalkTimer = 0;
                    spriteEffect = SpriteEffects.None;
                    animationTimer = 0;
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