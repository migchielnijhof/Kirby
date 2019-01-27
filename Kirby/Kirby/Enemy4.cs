using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy4 : Enemy
{

    protected const float jumpStrength = 2 * Game.SpriteScale;

    public static Texture2D sprite1;
    public static Texture2D sprite2;
    public static Texture2D sprite3;

    protected byte jumpTimer;
    protected byte airTimer;

    public Enemy4(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
        jumpTimer = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (!alive)
            return;
        if (onGround && jumpTimer < 100)
            jumpTimer++;
        if (!beingSucked)
        {
            if (onGround)
            {
                currentSprite = sprite1;
                airTimer = 0;
            }
            else
            {
                airTimer++;
                switch (airTimer)
                {
                    case 1:
                        currentSprite = sprite3;
                        break;
                    case 7:
                        currentSprite = sprite2;
                        break;
                    case 13:
                        currentSprite = sprite3;
                        break;
                    case 19:
                        currentSprite = sprite2;
                        break;
                }
            }
            Velocity.X = 0;
            if (jumpTimer == 100)
            {
                Velocity.Y = -jumpStrength;
                jumpTimer = 1;
            }
        }
        else
            currentSprite = sprite3;
        DoPhysics();
        beingSucked = false;
    }

}