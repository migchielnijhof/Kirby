using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

class AirPuff : PhysicsObject
{

    public const float Speed = 4 * Game.SpriteScale;

    public const int BoundingBoxX = (int)(16 * Game.SpriteScale);
    public const int BoundingBoxY = (int)(16 * Game.SpriteScale);

    public static Texture2D sprite;

    protected bool collided;

    protected byte lifeTime;
    protected float velocityMultiplier = 0.9f;

    protected SpriteEffects s;

    public AirPuff(GameObject parent) : base (parent, ObjectType.PlayerProjectile)
    {
        collided = false;
        lifeTime = 30;
        Gravity = 0;
        boundingBox.Size = new Point(BoundingBoxX, BoundingBoxY);
        s = SpriteEffects.None;
    }

    public override void Update(GameTime gameTime)
    {
        lifeTime--;
        Velocity.X *= velocityMultiplier;
        if (lifeTime == 0)
        {
            (parent as Level).Remove(this);
            collided = true;
        }
        DoPhysics();
        if (Velocity.X < 0)
        {
            s = SpriteEffects.FlipHorizontally;
        }
    }

    protected override bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        if (collided)
            return false;
        List<GameObject> enemies = (parent as Level).FindAll(ObjectType.Enemy);
        Rectangle Box = new Rectangle((int)Position.X, (int)Position.Y, BoundingBoxX, BoundingBoxY);
        foreach (Enemy e in enemies)
        {
            if (Box.Intersects(e.BoundingBox) && e.alive)
            {
                (parent as Level).Remove(this);
                collided = true;
                e.TakeHit(true);
                return false;
            }
        }
        if (base.MapCollisions(grid, objectMovement))
        {
            (parent as Level).Remove(this);
            collided = true;
        }
        return false;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprite, Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, s, 0);
    }

}