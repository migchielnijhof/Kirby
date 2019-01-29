using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

abstract class Enemy : PhysicsObject
{

    const int BoundingBoxSizeX = (int) (16 * Game.SpriteScale);
    const int BoundingBoxSizeY = (int) (16 * Game.SpriteScale);

    protected readonly ushort SuckScore;
    protected readonly ushort StarKill;
    protected readonly ushort PuffKill;
    protected readonly ushort PushKill;

    public bool beingSucked;

    public bool alive;

    public bool loaded;

    public bool succResistance;

    protected Texture2D currentSprite;

    protected Vector2 spriteSizeOffset = new Vector2 (0,0);

    public SpriteEffects spriteEffect;

    public static SoundEffect hitEffect;

    public Enemy(GameObject parent, ushort suckScore, ushort starKill, ushort puffKill, ushort pushKill) : base(parent, ObjectType.Enemy)
    {
        SuckScore = suckScore;
        StarKill = starKill;
        PuffKill = puffKill;
        PushKill = pushKill;
        spriteEffect = SpriteEffects.None;
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY);
        Gravity = 0.1f * Game.SpriteScale;
        beingSucked = false;
        alive = true;
        loaded = false;
        succResistance = false;
    }

    public void CheckForLoad()
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (this is WhispyWoods && p.Position.Y < 335 * Game.SpriteScale && alive)
        {
            loaded = false;
        }
        else if (p.Position.X > Position.X - (1800 / Game.SpriteScale))
        {
            loaded = true;
            if (this is Boss)
                p.level.cameraLocked = true;
        }
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (!alive)
            return;
        spriteBatch.Draw(currentSprite, Position - (parent as Level).CameraPosition + spriteSizeOffset * Game.SpriteScale, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, spriteEffect, 0);
    }

    public virtual void TakeHit(bool airPuff)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        alive = false;
        hitEffect.Play();
        if (airPuff)
        {
            p.score += PuffKill;
            EnemyDefeatParticle pa = new EnemyDefeatParticle(parent as Level);
            (parent as Level).Add(pa);
            if (Position.X > p.Position.X)
                pa.Position = new Vector2(Position.X + currentSprite.Width, Position.Y - 15 * Game.SpriteScale);
            else
            {
                pa.Position = new Vector2(Position.X - EnemyDefeatParticle.sprites[1].Width * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                pa.s = SpriteEffects.FlipHorizontally;
            }
        }
        else
            p.score += StarKill;
    }

    protected override bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        if (!alive)
            return false;
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (p.invulnerabilityTime <= 0 && p.BoundingBox.Intersects(BoundingBox))
            if (beingSucked && p.absorbedEnemy == null)
            {
                p.absorbedEnemy = this;
                p.score += SuckScore;
                alive = false;
                return false;
            }
            else
            {
                if (!beingSucked)
                {
                    p.TakeDamage(Position.X);
                    if (!(this is Boss))
                    {
                        alive = false;
                        EnemyDefeatParticle pa = new EnemyDefeatParticle(parent as Level);
                        (parent as Level).Add(pa);
                        if (Position.X > p.Position.X)
                            pa.Position = new Vector2(Position.X + currentSprite.Width, Position.Y - 15 * Game.SpriteScale);
                        else
                        {
                            pa.Position = new Vector2(Position.X - EnemyDefeatParticle.sprites[1].Width * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                            pa.s = SpriteEffects.FlipHorizontally;
                        }
                        p.score += PushKill;
                    }
                    return false;
                }
            }
        return base.MapCollisions(grid, objectMovement);
    }

}