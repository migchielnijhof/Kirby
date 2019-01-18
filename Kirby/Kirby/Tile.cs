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
    /// The width of a tile's sprite.
    /// </summary>
    public const int SpriteWidth = (int) (16 * Game.SpriteScale);

    /// <summary>
    /// The height of a tile's sprite.
    /// </summary>
    public const int SpriteHeight = (int) (16 * Game.SpriteScale);

    /// <summary>
    /// The names of tiles for level loading.
    /// </summary>
    public static char[] Names = new char[] { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '!', '@', '$', '%', '€', '^', '&', '(', ')', '_', '+', '=', '[', ']', '{', '}', 'ø', '¯', ',', 'þ', 'ü', 'í', 'ó', 'ö', '«', '»', 'ƒ', '©', '´', '×', '¿', 'ç', '¤', '¡', '¥', '®', 'á' };

    /// <summary>
    /// The tiles that are solid;
    /// </summary>
    protected static char[] SolidTiles = new char[] { 'r', 's', 'u', 'p', 'q', 'y', 'z', 'B', 'A' };

    /// <summary>
    /// The sprites of all tiles.
    /// </summary>
    public static Texture2D[] sprites = new Texture2D[Names.Length];

    /// <summary>
    /// Creates a new tile.
    /// </summary>
    public Tile(byte tileID)
    {
        ID = tileID; //Sets the ID for the tile
        Solid = false; //Not solid, unless proven otherwise
        for (int i = 0; i < SolidTiles.Length; i++)
            if (Names[ID] == SolidTiles[i]) //Makes the tile solid if it should be solid
            {
                Solid = true;
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