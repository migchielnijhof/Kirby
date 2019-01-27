using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy6Cap : Enemy
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    public static Texture2D sprite;

    public Enemy6Cap(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}