using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/// <summary>
/// A class for representing tiles in the game.
/// </summary>
class Tile
{

    /// <summary>
    /// The tile's sprite.
    /// </summary>
    protected Texture2D sprite;

    /// <summary>
    /// Whether the tile can be jumped through or not.
    /// </summary>
    public bool JumpThrough;

    /// <summary>
    /// The width of a tile's sprite.
    /// </summary>
    public const byte spriteWidth = 16;

    /// <summary>
    /// The height of a tile's sprite.
    /// </summary>
    public const byte spriteHeight = 16;

    /// <summary>
    /// Creates a new tile.
    /// </summary>
    public Tile()
    {
        JumpThrough = false;
        sprite = null;
    }

    /// <summary>
    /// Draws the tile at the given position.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (sprite != null)
            spriteBatch.Draw(sprite, position, Color.White);
    }

}