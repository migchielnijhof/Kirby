using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy : PhysicsObject
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    const int BoundingBoxSizeX = (int) (16 * Game.SpriteScale);
    const int BoundingBoxSizeY = (int) (16 * Game.SpriteScale);

    public bool beingSucked;

    public bool alive;

    public Enemy(GameObject parent) : base(parent, ObjectType.Enemy)
    {
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY);
        beingSucked = false;
        alive = true;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (!alive)
            return;
        spriteBatch.Draw(Player.playerSprites[0], Position - (parent as Level).CameraPosition, null, Color.DarkRed, 0, Vector2.Zero, Game.SpriteScale, SpriteEffects.None, 0);
    }
    public override void Update(GameTime gameTime)
    {
        if (!alive)
            return;
        if (!beingSucked)
            Velocity.X = -movementSpeed;
        DoPhysics();
        beingSucked = false;
    }

    protected override bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        if (!alive)
            return false;
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (p.BoundingBox.Intersects(BoundingBox))
            if (beingSucked && p.absorbedEnemy == null)
            {
                p.absorbedEnemy = this;
                alive = false;
                return false;
            }
            else
                p.TakeDamage();
        return base.MapCollisions(grid, objectMovement);
    }

}