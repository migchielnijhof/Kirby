﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class EnemyDefeatParticle : PhysicsObject
{
    public static Texture2D[] sprites = new Texture2D[7];
    public byte timer;
    public SpriteEffects s;

    public EnemyDefeatParticle(GameObject parent) : base(parent, ObjectType.Particle)
    {
        timer = 0;
        Gravity = 0;
        s = 0;
    }

    public override void Update(GameTime gameTime)
    {
        timer++;
        if (timer == 13)
        {
            (parent as Level).Remove(this);
        }
        DoPhysics();
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprites[timer / 2], Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, s, 0);
    }
}
