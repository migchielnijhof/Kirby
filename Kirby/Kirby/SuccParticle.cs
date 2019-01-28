using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class SuccParticle : PhysicsObject
{
    public static Texture2D sprite;
    public byte lifeTimer;

   public SuccParticle (GameObject parent) : base(parent, ObjectType.Particle)
    {
        lifeTimer = 0;
        Gravity = 0;
    }

    public override void Update(GameTime gameTime)
    {
        lifeTimer++;
        if (lifeTimer == 10)
            (parent as Level).Remove(this);
        DoPhysics();
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprite, Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, SpriteEffects.None, 0);
    }
}
