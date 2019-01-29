using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class BossDefeatParticle : PhysicsObject
{
    public static Texture2D[] sprites = new Texture2D[20];
    public byte timer;
    public SpriteEffects s;

    public BossDefeatParticle(GameObject parent) : base(parent, ObjectType.Particle)
    {
        timer = 2;
        Gravity = 0;
        s = 0;
    }

    public override void Update(GameTime gameTime)
    {
        timer++;
        if (timer == 37)
        {
            (parent as Level).Remove(this);
        }
        DoPhysics();
        Console.WriteLine(timer);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprites[timer / 2], Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, s, 0);
    }
}
