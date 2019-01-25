using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Media;

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
    const float cameraFollowing = 0.55f;

    /// <summary>
    /// Path to level files.
    /// </summary>
    const string path = "Content/levels/";

    public Song greenGreens;

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
        reader.Close();

        byte columns = 0;

        for (ushort i = 0; i < instructions[0].Length; i++)
        {
            if (instructions[0][i] == '_' || i == instructions[0].Length - 1)
                columns++;
        }

        TileGrid grid = new TileGrid(this, columns, (byte) instructions.Count);

        ushort startIndex = 0;
        byte tileX = 0;

        for (byte y = 0; y < instructions.Count; y++)
        {
            for (ushort x = 0; x < instructions[y].Length; x++)
            {
                if (instructions[y][x] == '_')
                {
                    grid.tiles[tileX, y] = new Tile(byte.Parse(instructions[y].Substring(startIndex, x - startIndex)));
                    tileX++;
                    startIndex = x;
                    startIndex++;
                }
                else if (x == instructions[y].Length - 1)
                {
                    grid.tiles[tileX, y] = new Tile(byte.Parse(instructions[y].Substring(startIndex, x - startIndex)));
                    tileX++;
                }
            }
            tileX = 0;
            startIndex = 0;
        }
        Add(grid);

        reader = new StreamReader(path + level + "-.txt");
        nextLine = reader.ReadLine();
        Enemy e = null;
        while (nextLine != null)
        {
            switch (nextLine[0])
            {
                case 'W':
                    e = //new Enemy(this);
                        null;
                    break;
                case 'F':
                    e = null;
                    break;
                case 'B':
                    e = null;
                    break;
                case 'J':
                    e = null;
                    break;
                case 'M':
                    e = null;
                    break;
                case 'X':
                    e = null;
                    break;
                case 'L':
                    e = null;
                    break;
                case 'G':
                    e = null;
                    break;
                case 'S':
                    e = null;
                    break;
                default:
                    e = null;
                    break;
            }
            if (e != null)
            {
                e.Position = new Vector2(Tile.SpriteWidth * byte.Parse(nextLine.Substring(1, 3)), Tile.SpriteHeight * byte.Parse(nextLine.Substring(4, 2)));
                Add(e);
            }
            nextLine = reader.ReadLine();
        }
        reader.Close();

        Player p = new Player(this);
        Add(p);
        Add(new UI(p));
        MediaPlayer.Play(greenGreens);
    }

}