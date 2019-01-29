using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class WarpStar : PhysicsObject
{
    public static Texture2D[] sprites = new Texture2D[7];
    public byte timer;
    public SpriteEffects s;

    public WarpStar(GameObject parent) : base(parent, ObjectType.Particle)
    {
        timer = 1;
        Gravity = 0;
        s = 0;
    }

    public override void Update(GameTime gameTime)
    {
        timer++;
        if (timer == 15)
        {
            timer = 1;
        }
        DoPhysics();
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprites[timer / 4 + 1], Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, s, 0);
    }
}
