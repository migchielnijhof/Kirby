using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class Apple : Enemy
{

    protected const float movementSpeed = 2.4f * Game.SpriteScale;

    public static Texture2D sprite;

    public bool hitFloor;

    public Apple(GameObject parent) : base(parent, 10, 400, 400, 400)
    {
        currentSprite = sprite;
        Gravity = 0.2f;
        hitFloor = false;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive)
        {
            DoPhysics();
            beingSucked = false;
            if (onGround)
            {
                if (!hitFloor)
                {
                    hitFloor = true;
                    Random random = new Random();
                    int direction = random.Next(2);
                    if (direction == 1)
                        Velocity.X = 4;
                    else
                        Velocity.X = -4;
                    Velocity.Y = -4;
                }
                else
                {
                    alive = false;
                    (parent as Level).Remove(this);
                }
            }
        }
    }

}