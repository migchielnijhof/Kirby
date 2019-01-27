using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy7 : Enemy
{

    protected const float flySpeed = 1 * Game.SpriteScale;

    public static Texture2D sprite1;
    public static Texture2D sprite2;

    protected byte flyTimer;
    protected byte animationTimer;

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public Enemy7(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
        flyTimer = 0;
        Gravity = 0;
        animationTimer = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (!beingSucked)
            {
                Velocity.X = -movementSpeed;
                flyTimer++;
                animationTimer++;
                switch (flyTimer)
                {
                    case 1:
                        Velocity.Y = flySpeed / 2;
                        break;
                    case 7:
                        Velocity.Y = flySpeed;
                        break;
                    case 19:
                        Velocity.Y = flySpeed / 2;
                        break;
                    case 25:
                        Velocity.Y = -flySpeed / 2;
                        break;
                    case 31:
                        Velocity.Y = -flySpeed;
                        break;
                    case 43:
                        Velocity.Y = -flySpeed / 2;
                        break;
                }
                if (flyTimer == 49)
                {
                    flyTimer = 0;
                }
                if (animationTimer == 10)
                {
                    currentSprite = sprite2;
                }
                else if (animationTimer == 20)
                {
                    animationTimer = 0;
                    currentSprite = sprite1;
                }
            }
            else
                currentSprite = sprite1;
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}