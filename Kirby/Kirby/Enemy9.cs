using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy9 : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public static Texture2D sprite1;

    public static Texture2D sprite2;

    protected byte WalkTimer;

    protected byte animationTimer;

    public Enemy9(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        WalkTimer = 0;
        Gravity = 0.04f * Game.SpriteScale;
    }

    public override void Update(GameTime gameTime)
    {
        if (!alive)
            return;
        if (!beingSucked)
        {
            if (animationTimer < 8)
                animationTimer++;
            else
            {
                animationTimer = 0;
            }
            if (WalkTimer < 50)
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
            if (WalkTimer == 100)
                WalkTimer = 0;
            if (onGround)
                Velocity.Y = -2;
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

}