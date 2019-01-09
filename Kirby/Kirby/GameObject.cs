using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A class for representing an object that can be updated, handle input and be drawn in the game loop.
/// </summary>
class GameObject
{

    /// <summary>
    /// The game object can communicate with other game objects through the parent.
    /// </summary>
    protected GameObject parent;

    /// <summary>
    /// The type of gameobject.
    /// </summary>
    public readonly ObjectType Type;

    /// <summary>
    /// A type of gameobject.
    /// This enum allows for distinguishing different types of game objects as well as updating and drawing them in the order that is defined here.
    /// </summary>
    public enum ObjectType { Level, Background, TileGrid, Enemy, Player, Boss };

    /// <summary>
    /// Creates a game loop object.
    /// </summary>
    /// <param name="parent">The game object can communicate with other game objects through the parent.</param>
    /// <param name="type">The type of gameobject.</param>
    public GameObject(GameObject parent, ObjectType type)
    {
        this.parent = parent;
        Type = type;
    }

    public virtual void HandleInput(Input input)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
    }

}
