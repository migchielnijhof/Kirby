using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/// <summary>
/// A class for representing tiles in the game.
/// </summary>
class Tile
{

    /// <summary>
    /// The tile's numeric ID.
    /// </summary>
    protected byte ID;

    /// <summary>
    /// Whether this tile is solid or not.
    /// </summary>
    public bool Solid;

    /// <summary>
    /// Whether you can jump through this tile or not;
    /// </summary>
    public bool JumpThrough;

    /// <summary>
    /// The width of a tile's sprite.
    /// </summary>
    public const int SpriteWidth = (int)(16 * Game.SpriteScale);

    /// <summary>
    /// The height of a tile's sprite.
    /// </summary>
    public const int SpriteHeight = (int)(16 * Game.SpriteScale);

    /// <summary>
    /// The highest tile ID;
    /// </summary>
    public const byte MaxID = 115;

    /// <summary>
    /// The tiles that are solid.
    /// </summary>
    protected static byte[] SolidTiles = new byte[] { 25, 26, 27, 28, 30, 34, 35, 36, 37, 41, 101, 104, 105, 106, 107 };

    /// <summary>
    /// The tiles that can be jumped through.
    /// </summary>
    protected static byte[] JumpThroughTiles = new byte[] { 103, 108 };

    /// <summary>
    /// The sprites of all tiles.
    /// </summary>
    public static Texture2D[] sprites = new Texture2D[MaxID + 1];

    /// <summary>
    /// Creates a new tile.
    /// </summary>
    public Tile(byte tileID)
    {
        ID = tileID; //Sets the ID for the tile
        Solid = false;
        JumpThrough = false;
        for (int i = 0; i < SolidTiles.Length; i++)
            if (ID == SolidTiles[i]) //Makes the tile solid if it should be solid
            {
                Solid = true;
                break;
            }
        for (int i = 0; i < JumpThroughTiles.Length; i++)
            if (ID == JumpThroughTiles[i]) //Makes the tile jumpthrough if it should be
            {
                JumpThrough = true;
                break;
            }
    }

    /// <summary>
    /// Draws the tile at the given position.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(sprites[ID], position, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);
    }

}