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
        Health = maxHealth; //The player starts with maximum health
        score = 0; //The player starts without a score
        absorbedEnemy = null; //The player starts without an enemy absorbed
        invulnerabilityTime = 0.0d; //The player starts without invulnerability
        Velocity = Vector2.Zero; //The player starts without any velocity
        onGround = true; //The player starts on the ground
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY); //Sets the bounding box size
    }

    public void TakeDamage()
    {
        if (invulnerabilityTime > 0) //The player shouldn't receive damage if they're still invulnerable
            return;
        Health--; //Gets hit
        if (Health == 0) //If you don't have any health
        {
            Die(); //You die
            return;
        }
        invulnerabilityTime = invulnerability; //gives the player invulnerability
    }

    protected void Die()
    {

    }
    
    //test

    public override void HandleInput(Input input)
    {
        Velocity.X = 0;
        if (input.Movement == 1)
            Velocity.X = movementSpeed;
        else if (input.Movement == 2)
            Velocity.X = -movementSpeed;
        if (input.Jump) //Jumps when you press the jump key
        {
            Velocity.Y = -200 * Game.SpriteScale;
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (!onGround) //Sends the player down if they're not on the ground
            Velocity.Y += gravity;
        else if (Velocity.Y > 0) //The player can't move on the Y axis when on the ground.
            Velocity.Y = 0;
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Position.X < 0) //The player can't move past the left edge of the screen
            Position.X = 0;
        if (Position.Y < 0) //The player can't move past the top edge of the screen
            Position.Y = 0;
        TileGrid grid = (parent as Level).Find(ObjectType.TileGrid) as TileGrid;
        try //Collision with the tilegrid
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