using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy : PhysicsObject
{

    protected const float movementSpeed = -0.6f * Game.SpriteScale;

    const int BoundingBoxSizeX = (int) (16 * Game.SpriteScale);
    const int BoundingBoxSizeY = (int) (16 * Game.SpriteScale);

    public Enemy(GameObject parent) : base(parent, ObjectType.Enemy)
    {
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Player.playerSprites[0], Position - (parent as Level).CameraPosition, null, Color.DarkRed, 0, Vector2.Zero, Game.SpriteScale, SpriteEffects.None, 0);
    }

    public override void HandleInput(Input input)
    {
        Velocity.X = movementSpeed;
        base.HandleInput(input);
        if (input.Jump)
            Velocity.X = -movementSpeed;
    }

    public override void Update(GameTime gameTime)
    {
        DoPhysics();
    }

    protected override bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (p.BoundingBox.Intersects(BoundingBox))
            p.TakeDamage();
        return base.MapCollisions(grid, objectMovement);
    }

}