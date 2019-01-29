using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class BossPuff : Enemy
{

    protected const float movementSpeed = 2.4f * Game.SpriteScale;

    public static Texture2D sprite1;

    public BossPuff(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        spriteEffect = SpriteEffects.FlipHorizontally;
        Gravity = 0.1f;
        succResistance = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && Position.X > 10)
        {
            beingSucked = false;
            Velocity.X = -movementSpeed;
            DoPhysics();
        }
        else
        {
            alive = false;
            (parent as Level).Remove(this);
        }
    }

}