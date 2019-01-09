using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// A class for representing game levels.
/// </summary>
class Level : GameObjectList
{

    /// <summary>
    /// The currently loaded level.
    /// </summary>
    public byte CurrentLevel;

    /// <summary>
    /// Position of the level's camera.
    /// </summary>
    public Vector2 CameraPosition;

    /// <summary>
    /// The camera will move when the position of the player on the screen becomes higher than cameraFollowing * screen size or lower than (1 - cameraFollowing) * screen size.
    /// Higher values allow the player to be further off the center of the screen.
    /// Value should be more than 0.5f and less than 1.0f.
    /// </summary>
    const float cameraFollowing = 0.7f;

    /// <summary>
    /// Path to level files.
    /// </summary>
    const string path = "Content/levels/";

    /// <summary>
    /// Create a level.
    /// </summary>
    public Level(GameObject parent) : base(parent, ObjectType.Level)
    {
        CurrentLevel = 0;
        Clear();
    }

    /// <summary>
    /// Updates the level.
    /// </summary>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Update camera position based on the position of the player.
        if (Find(ObjectType.Player) is Player p)
        {
            // Player too far to the right.
            if ((p.Position.X - CameraPosition.X) > Game.graphics.PreferredBackBufferWidth * cameraFollowing)
                CameraPosition.X = Game.graphics.PreferredBackBufferWidth * cameraFollowing - (p.Position.X - CameraPosition.X);
            // Player too far to the left.
            else if ((p.Position.X - CameraPosition.X) < Game.graphics.PreferredBackBufferWidth * (1 - cameraFollowing))
                CameraPosition.X = Game.graphics.PreferredBackBufferWidth * (1 - cameraFollowing) - (p.Position.X - CameraPosition.X);
        }
        // Beginning of the level displaying somewhere else than the far left of the screen.
        if (CameraPosition.X < 0)
            CameraPosition.X = 0;
        else
        {
            // End of the level displaying somewhere else than the far right of the screen.
            if (Find(ObjectType.TileGrid) is TileGrid t && CameraPosition.X + Game.graphics.PreferredBackBufferWidth > t.width * Tile.spriteWidth)
                CameraPosition.X = t.width * Tile.spriteWidth - Game.graphics.PreferredBackBufferWidth;
        }
    }

    /// <summary>
    /// Loads a new level.
    /// </summary>
    /// <param name="level">The level to load.</param>
    public void Load(byte level)
    {
        Clear();
        CurrentLevel = level;
        List<string> instructions = new List<string>();
        StreamReader reader = new StreamReader(path + level + ".txt");
        string nextLine = reader.ReadLine();
        while (nextLine != null)
        {
            instructions.Add(nextLine);
            nextLine = reader.ReadLine();
        }

        Add(new TileGrid(this, (byte) instructions[0].Length, (byte) instructions.Count));

        for (byte y = 0; y < instructions.Count; y++)
            for (byte x = 0; x < instructions[y].Length; x++)
            {
                switch (instructions[y][x])
                {
                    case 'P':
                        Player p = new Player(this)
                        {
                            Position = new Vector2(x * Tile.spriteWidth, y * Tile.spriteHeight)
                        };
                        Add(p);
                        break;
                }
            }

    }

}