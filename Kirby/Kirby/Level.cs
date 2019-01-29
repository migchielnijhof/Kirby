using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework.Media;

/// <summary>
/// A class for representing game levels.
/// </summary>
class Level : GameObjectList
{

    /// <summary>
    /// The currently loaded level.
    /// </summary>
    public byte currentLevel;

    /// <summary>
    /// Health of the boss that might be on screen.
    /// </summary>
    public byte bossHealth;

    /// <summary>
    /// All the warps in the current level.
    /// </summary>
    public List<Door> Doors;

    /// <summary>
    /// Position of the level's camera.
    /// </summary>
    public Vector2 CameraPosition;

    /// <summary>
    /// If the camera can be moved.
    /// </summary>
    public bool cameraLocked = false;

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
        Doors = new List<Door>();
        currentLevel = 0;
        Clear();
    }

    /// <summary>
    /// Updates the level.
    /// </summary>
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateCamera();
    }

    /// <summary>
    /// Updates the camera position based on the player's position.
    /// </summary>
    public void UpdateCamera()
    {
        if (Find(ObjectType.Player) is Player p && !cameraLocked)
        {
            // Player too far to the right.
            if ((p.Position.X - CameraPosition.X) > Game.ScreenWidth * Game.SpriteScale * cameraFollowing)
                CameraPosition.X = p.Position.X - Game.ScreenWidth * Game.SpriteScale * cameraFollowing;
            // Player too far to the left.
            else if ((p.Position.X - CameraPosition.X) < Game.ScreenWidth * Game.SpriteScale * (1 - cameraFollowing))
                CameraPosition.X = p.Position.X - Game.ScreenWidth * Game.SpriteScale * (1 - cameraFollowing);
            // Player too far to the right.
            if ((p.Position.Y - CameraPosition.Y) > Game.ScreenHeight * Game.SpriteScale * cameraFollowing)
                CameraPosition.Y = p.Position.Y - Game.ScreenHeight * Game.SpriteScale * cameraFollowing;
            // Player too far to the left.
            else if ((p.Position.Y - CameraPosition.Y) < Game.ScreenHeight * Game.SpriteScale * (1 - cameraFollowing))
                CameraPosition.Y = p.Position.Y - Game.ScreenHeight * Game.SpriteScale * (1 - cameraFollowing);
        }
        // Beginning of the level displaying somewhere else than the far left of the screen.
        if (CameraPosition.X < 0)
            CameraPosition.X = 0;
        else
        {
            // End of the level displaying somewhere else than the far right of the screen.
            if (Find(ObjectType.TileGrid) is TileGrid t && CameraPosition.X + Game.ScreenWidth * Game.SpriteScale > t.width * Tile.SpriteWidth && !cameraLocked)
                CameraPosition.X = t.width * Tile.SpriteWidth - Game.ScreenWidth * Game.SpriteScale;
        }
        if (CameraPosition.Y < 0)
            CameraPosition.Y = 0;
        else
        {
            // End of the level displaying somewhere else than the far right of the screen.
            if (Find(ObjectType.TileGrid) is TileGrid t && CameraPosition.Y + Game.ScreenHeight * Game.SpriteScale - 16 * Game.SpriteScale > t.height * Tile.SpriteHeight && !cameraLocked)
                CameraPosition.Y = t.height * Tile.SpriteHeight - Game.ScreenHeight * Game.SpriteScale + 16 * Game.SpriteScale;
        }
    }

    /// <summary>
    /// Loads a new level.
    /// </summary>
    /// <param name="level">The level to load.</param>
    public void Load(bool newLevel, byte level, Player p)
    {
        Clear();
        currentLevel = level; //Sets the current level
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
        
        Add(p);

        reader = new StreamReader(path + level + "-.txt");
        nextLine = reader.ReadLine();
        PhysicsObject e = null;
        while (nextLine != null)
        {
            switch (nextLine[0])
            {
                case 'W':
                    e = new Enemy1(this);
                    break;
                case 'F':
                    e = new Enemy2(this);
                    break;
                case 'B':
                    e = new Enemy3(this);
                    break;
                case 'J':
                    e = new Enemy4(this);
                    break;
                case 'U':
                    e = new Enemy5(this);
                    break;
                case 'M':
                    e = new Enemy6(this);
                    break;
                case 'X':
                    e = new Enemy7(this);
                    break;
                case 'L':
                    e = new Enemy8(this);
                    break;
                case 'G':
                    e = new Enemy9(this);
                    break;
                case 'S':
                    e = null;
                    Doors.Add(new Door(byte.Parse(nextLine.Substring(1, 3)), byte.Parse(nextLine.Substring(4, 2)), byte.Parse(nextLine.Substring(6, 1)), byte.Parse(nextLine.Substring(7, 3)), byte.Parse(nextLine.Substring(10, 2))));
                    break;
                case 'H':
                    e = null;
                    break;
                case 'I':
                    e = new PopoBrosSr(this);
                    break;
                case 'A':
                    e = new Enemy11(this);
                    break;
                case 'E':
                    e = new Enemy12(this);
                    break;
                case 'D':
                    e = null;
                    break;
                case 'O':
                    e = //new Block(this);
                        null;
                    break;
                case 'R':
                    e = new Enemy14(this);
                    break;
                case 'C':
                    e = new Enemy15(this);
                    break;
                case 'T':
                    e = null;
                    break;
                case 'K':
                    e = null;
                    break;
                case 'P':
                    e = null;
                    Doors.Add(new Door(byte.Parse(nextLine.Substring(1, 3)), byte.Parse(nextLine.Substring(4, 2)), byte.Parse(nextLine.Substring(6, 1)), byte.Parse(nextLine.Substring(7, 3)), byte.Parse(nextLine.Substring(10, 2))));
                    break;
                case 'Y':
                    e = new WhispyWoods(this);
                    break;
                case 'Q':
                    e = new BossHitBox(this);
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

        Add(new UI(p));

        if (newLevel)
            MediaPlayer.Play(greenGreens);
    }

}