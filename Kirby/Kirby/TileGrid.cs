﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing a grid of tiles.
/// </summary>
class TileGrid : GameObject
{

    /// <summary>
    /// The array of tiles in this tilegrid.
    /// </summary>
    public Tile[,] tiles;

    /// <summary>
    /// The width of this tilegrid.
    /// </summary>
    public byte width;

    /// <summary>
    /// The height of this tilegrid.
    /// </summary>
    public byte height;

    /// <summary>
    /// Creates a tile grid.
    /// </summary>
    /// <param name="parent">The game object can communicate with other game objects through the parent.</param>
    /// <param name="instructions">The instructions to load the tile grid.</param>
    public TileGrid(Level parent, byte width, byte height) : base(parent, ObjectType.TileGrid)
    {
        this.width = width;
        this.height = height;
        tiles = new Tile[width, height];
    }

    /// <summary>
    /// Retrieves the x index of a position x on the tilegrid.
    /// </summary>
    /// <param name="x">The x position.</param>
    public byte GetIndexX(float x)
    {
        return (byte) ((x - 0.5) / Tile.SpriteWidth);
    }

    /// <summary>
    /// Retrieves the y index of a position y on the tilegrid.
    /// </summary>
    /// <param name="x">The y position.</param>
    public byte GetIndexY(float y)
    {
        return (byte) ((y - 0.5) / Tile.SpriteHeight);
    }

    /// <summary>
    /// Retrieves the bounding box of a tile at (x, y).
    /// </summary>
    /// <param name="x">x index of the tile.</param>
    /// <param name="y">y index of the tile.</param>
    /// <returns></returns>
    public Rectangle GetBoundingBox(int x, int y)
    {
        return new Rectangle(new Point(x * Tile.SpriteWidth, y * Tile.SpriteHeight), new Point(Tile.SpriteWidth, Tile.SpriteHeight));
    }

    /// <summary>
    /// Draws all the tiles of this tilegrid on the screen.
    /// </summary>
    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y].Draw(spriteBatch, new Vector2(Tile.SpriteWidth * x, Tile.SpriteHeight * y) - (parent as Level).CameraPosition);
    }

}