using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Bomb : Enemy
{

    protected const float movementSpeed = 2.4f * Game.SpriteScale;

    public static Texture2D sprite;

    public Bomb(GameObject parent) : base(parent, 10, 400, 400, 400)
    {
        currentSprite = sprite;
        Velocity.X = -movementSpeed;
        Gravity = 0;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive)
        {
            DoPhysics();
            beingSucked = false;
            Console.WriteLine(Position.X);
            if (Position.X < 5)
            {
                alive = false;
                (parent as Level).Remove(this);
            }
                
        }
    }

}