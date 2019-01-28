using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy3 : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public static Texture2D sprite1;
    public static Texture2D sprite2;
    protected byte movementTimer;

    public Enemy3(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
        Gravity = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (!beingSucked)
            {
                Velocity.X = -movementSpeed;
                Velocity.Y = (Velocity.Y / 5 * 4);
                movementTimer++;
                if (movementTimer > 20)
                {
                    movementTimer = 0;
                    if (currentSprite == sprite1)
                    {
                        Velocity.Y = 10;
                        currentSprite = sprite2;
                    }
                    else
                    {
                        Velocity.Y = -10;
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