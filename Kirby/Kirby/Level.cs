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

    private Player player;

    /// <summary>
    /// The camera will move when the position of the player on the screen becomes higher than cameraFollowing * screen size or lower than (1 - cameraFollowing) * screen size.
    /// Higher values allow the player to be further off the center of the screen.
    /// Value should be more than 0.5f and less than 1.0f.
    /// </summary>
    const float cameraFollowing = 0.55f;

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
            if ((p.Position.X - CameraPosition.X) > Game.ScreenWidth * Game.SpriteScale * cameraFollowing)
                CameraPosition.X = p.Position.X - Game.ScreenWidth * Game.SpriteScale * cameraFollowing;
            // Player too far to the left.
            else if ((p.Position.X - CameraPosition.X) < Game.ScreenWidth * Game.SpriteScale * (1 - cameraFollowing))
                CameraPosition.X = p.Position.X - Game.ScreenWidth * Game.SpriteScale * (1 - cameraFollowing);
        }
        // Beginning of the level displaying somewhere else than the far left of the screen.
        if (CameraPosition.X < 0)
            CameraPosition.X = 0;
        else
        {
            // End of the level displaying somewhere else than the far right of the screen.
            if (Find(ObjectType.TileGrid) is TileGrid t && CameraPosition.X + Game.ScreenWidth > t.width * Tile.SpriteWidth)
                CameraPosition.X = t.width * Tile.SpriteWidth - Game.ScreenWidth;
        }
    }

    /// <summary>
    /// Loads a new level.
    /// </summary>
    /// <param name="level">The level to load.</param>
    public void Load(byte level)
    {
        Clear();
        CurrentLevel = level; //Sets the current level
        List<string> instructions = new List<string>();
        StreamReader reader = new StreamReader(path + level + ".txt"); //Loads the file that contains the level
        string nextLine = reader.ReadLine();
        while (nextLine != null) //Reads the line until it ends
        {
            instructions.Add(nextLine);
            nextLine = reader.ReadLine();
        }

        TileGrid grid = new TileGrid(this, (byte) instructions[0].Length, (byte) instructions.Count);

        for (byte y = 0; y < instructions.Count; y++)
            for (byte x = 0; x < instructions[y].Length; x++)
            {
                for (byte i = 0; i < Tile.Names.Length; i++)
                {
                    System.Console.WriteLine(instructions[y][x]);
                    if (instructions[y][x] == Tile.Names[i])
                    {
                        grid.tiles[x, y] = new Tile(i); //Sets a tile for each character in the level file
                        break;
                    }
                }
            }
        Add(grid);
        player = new Player(this);
        Add(player);
        Add(new UI(player));
    }

}