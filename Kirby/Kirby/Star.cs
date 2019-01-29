using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

class Star : PhysicsObject
{

    public const float Speed = 3 * Game.SpriteScale;

    public const int BoundingBoxX = (int)(15 * Game.SpriteScale);
    public const int BoundingBoxY = (int)(15 * Game.SpriteScale);

    public static Texture2D[] sprites = new Texture2D[4];

    protected byte spriteID;

    protected bool collided;

    protected byte animationTimer;

    protected SpriteEffects s;

    public Star(GameObject parent) : base(parent, ObjectType.PlayerProjectile)
    {
        collided = false;
        Gravity = 0;
        boundingBox.Size = new Point(BoundingBoxX, BoundingBoxY);
        spriteID = 0;
        animationTimer = 0;
        s = SpriteEffects.None;
    }

    public override void Update(GameTime gameTime)
    {
        DoPhysics();
        if (animationTimer == 3)
            animationTimer = 0;
        else
            animationTimer++;
        if (animationTimer == 0)
        {
            if (spriteID == 3)
                spriteID = 0;
            else
                spriteID++;
        }
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
            if (e.alive && Box.Intersects(e.BoundingBox))
            {
                (parent as Level).Remove(this);
                EnemyDefeatParticle p = new EnemyDefeatParticle(parent as Level);
                (parent as Level).Add(p);
                if (Velocity.X > 0)
                    p.Position = new Vector2(Position.X + sprites[1].Width, Position.Y - 15 * Game.SpriteScale);
                else {
                    p.Position = new Vector2(Position.X - EnemyDefeatParticle.sprites[1].Width * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                    p.s = SpriteEffects.FlipHorizontally;
                }
                collided = true;
                e.TakeHit(false);
                return false;
            }
        }
        if (base.MapCollisions(grid, objectMovement))
        {
            (parent as Level).Remove(this);
            EnemyDefeatParticle p = new EnemyDefeatParticle(parent as Level);
            (parent as Level).Add(p);
            if (Velocity.X > 0)
                p.Position = new Vector2(Position.X + sprites[1].Width, Position.Y - 15 * Game.SpriteScale);
            else
            {
                p.Position = new Vector2(Position.X - EnemyDefeatParticle.sprites[1].Width * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                p.s = SpriteEffects.FlipHorizontally;
            }
            collided = true;
        }
        return false;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(sprites[spriteID], Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, SpriteEffects.None, 0);
    }

}