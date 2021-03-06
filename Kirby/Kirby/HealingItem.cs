﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

class HealingItem : PhysicsObject
{
    public const int BoundingBoxX = (int)(15 * Game.SpriteScale);
    public const int BoundingBoxY = (int)(15 * Game.SpriteScale);

    public static Texture2D sprite;

    public static SoundEffect soundEffect;

    public HealingItem(GameObject parent) : base(parent, ObjectType.PlayerProjectile)
    {
        Gravity = 0;
        boundingBox.Size = new Point(BoundingBoxX, BoundingBoxY);
    }

    public override void Update(GameTime gameTime)
    {
        DoPhysics();
    }

    protected override bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        Rectangle Box = new Rectangle((int)Position.X, (int)Position.Y, BoundingBoxX, BoundingBoxY);
            if (Box.Intersects(p.BoundingBox))
            {
                (parent as Level).Remove(this);
            p.Health += 2;
            if (p.Health > Player.maxHealth)
            {
                p.Health = Player.maxHealth;
            }
            soundEffect.Play();
            return false;
            }
        return false;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprite, Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);
    }

}