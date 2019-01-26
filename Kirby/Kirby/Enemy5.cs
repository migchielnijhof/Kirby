using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy5 : Enemy
{

    protected const float flySpeed = 1 * Game.SpriteScale;

    public static Texture2D sprite1;
    public static Texture2D sprite2;

    protected byte flyTimer;
    protected byte animationTimer;

    public Enemy5(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
        flyTimer = 0;
        Gravity = 0;
        animationTimer = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (!alive)
            return;
        if (!beingSucked)
        {
            Velocity.X = 0;
            flyTimer++;
            animationTimer++;
            switch (flyTimer)
            {
                case 1:
                    Velocity.Y = flySpeed / 4;
                    break;
                case 17:
                    Velocity.Y = flySpeed / 2;
                    break;
                case 113:
                    Velocity.Y = flySpeed / 4;
                    break;
                case 129:
                    Velocity.Y = -flySpeed / 4;
                    break;
                case 145:
                    Velocity.Y = -flySpeed / 2;
                    break;
                case 239:
                    Velocity.Y = -flySpeed / 4;
                    break;
            }
            if (flyTimer == 255)
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

}