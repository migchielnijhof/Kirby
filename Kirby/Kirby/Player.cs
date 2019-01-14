using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// Class of the playable character.
/// </summary>
class Player : AnimatedGameObject
{

    /// <summary>
    /// Current health of the player.
    /// </summary>
    public byte Health;

    /// <summary>
    /// The position of the player.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The velocity of the player.
    /// </summary>
    public Vector2 Velocity;

    /// <summary>
    /// The bounding box of the player.
    /// </summary>
    public Rectangle BoundingBox
    {
        get
        {
            boundingBox.Location = new Point((int) Position.X, (int) Position.Y);
            return boundingBox;
        }
    }
    protected Rectangle boundingBox;

    /// <summary>
    /// The X size of the player's bounding box.
    /// </summary>
    const int BoundingBoxSizeX = (int) (16 * Game.SpriteScale);

    /// <summary>
    /// The Y size of the player's bounding box.
    /// </summary>
    const int BoundingBoxSizeY = (int) (16 * Game.SpriteScale);

    /// <summary>
    /// Maximum health of the player.
    /// </summary>
    const byte maxHealth = 5;

    /// <summary>
    /// The movement speed of a player.
    /// </summary>
    const float movementSpeed = 45 * Game.SpriteScale;

    /// <summary>
    /// The gravity of the player.
    /// </summary>
    const float gravity = 9 * Game.SpriteScale;

    /// <summary>
    /// The player's score.
    /// </summary>
    public int score;

    /// <summary>
    /// Whether the player is on the ground or not.
    /// </summary>
    protected bool onGround;

    /// <summary>
    /// The enemy the player has sucked up.
    /// </summary>
    protected GameObject absorbedEnemy;

    /// <summary>
    /// The amount of time the player will remain invulnerable.
    /// </summary>
    protected double invulnerabilityTime;

    /// <summary>
    /// The amount of invulnerabiliyTime the player will get after taking damage.
    /// </summary>
    const double invulnerability = 1.0d;

    /// <summary>
    /// Create a new player.
    /// </summary>
    /// <param name="parent">The game object can communicate with other game objects through the parent.</param>
    public Player(GameObject parent) : base(parent, ObjectType.Player)
    {
        Health = maxHealth;
        score = 0;
        absorbedEnemy = null;
        invulnerabilityTime = 0.0d;
        Velocity = Vector2.Zero;
        onGround = true;
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY);
    }

    public void TakeDamage()
    {
        if (invulnerabilityTime > 0)
            return;
        Health--;
        if (Health == 0)
        {
            Die();
            return;
        }
        invulnerabilityTime = invulnerability;
    }

    protected void Die()
    {

    }

    public override void HandleInput(Input input)
    {
        Velocity.X = 0;
        if (input.Movement == 1)
            Velocity.X = movementSpeed;
        else if (input.Movement == 2)
            Velocity.X = -movementSpeed;
        if (input.Jump)
        {
            Velocity.Y = -200 * Game.SpriteScale;
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (!onGround)
            Velocity.Y += gravity;
        else if (Velocity.Y > 0)
            Velocity.Y = 0;
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Position.X < 0)
            Position.X = 0;
        if (Position.Y < 0)
            Position.Y = 0;
        TileGrid grid = (parent as Level).Find(ObjectType.TileGrid) as TileGrid;
        try
        {
            TileCollision(grid, 0, 0);
            TileCollision(grid, 1, 0);
            TileCollision(grid, 0, 1);
            TileCollision(grid, 1, 1);
            onGround = TestGround(grid);
        }
        catch
        {
            TakeDamage();
        }
    }

    public bool TestGround(TileGrid grid)
    {
        byte x = grid.GetIndexX(Position.X);
        byte y = grid.GetIndexY(Position.Y + BoundingBox.Size.Y);
        return grid.tiles[x, y].Solid || grid.tiles[x + 1, y].Solid;
    }

    public void TileCollision(TileGrid grid, byte offsetX, byte offsetY)
    {
        byte x = (byte) (grid.GetIndexX(Position.X) + offsetX);
        byte y = (byte) (grid.GetIndexY(Position.Y) + offsetY);
        Rectangle tile = grid.GetBoundingBox(x, y);
        if (grid.tiles[x, y].Solid && (BoundingBox.Intersects(tile) || BoundingBox.Contains(tile) || tile.Contains(BoundingBox)))
        {
            Console.WriteLine("i");
            int a = BoundingBox.Center.X - tile.Center.X;
            int b = BoundingBox.Center.Y - tile.Center.Y;
            if (Math.Abs(a) > Math.Abs(b))
            {
                if (a > 0)
                    Position.X = tile.X + tile.Size.X;
                else
                    Position.X = tile.X - BoundingBox.Size.X;
                Velocity.X = 0;
            }
            else
            {
                if (b > 0)
                    Position.Y = tile.Y + tile.Size.Y;
                else
                    Position.Y = tile.Y - BoundingBox.Size.Y;
                Velocity.Y = 0;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Tile.sprites[5], Position - (parent as Level).CameraPosition, null, Color.Red, 0, Vector2.Zero, Game.SpriteScale, 0, 0);
    }

}