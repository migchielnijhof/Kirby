using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/// <summary>
/// An objectlist contains game objects and enables game objects to find other game objects in the list.
/// </summary>
class GameObjectList : GameObject
{
    /// <summary>
    /// All objects in the list
    /// </summary>
    List<GameObject> objects;

    public GameObjectList(GameObject parent, ObjectType type) : base(parent, type)
    {
        objects = new List<GameObject>();
    }

    /// <summary>
    /// Adds a game object to this list.
    /// </summary>
    /// <param name="o">The game object to add.</param>
    public void Add(GameObject o)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].Type <= o.Type)
            {
                objects.Insert(i, o);
                return;
            }
        }
        objects.Add(o);
    }


    /// <summary>
    /// Removes a game object from this list.
    /// </summary>
    /// <param name="o">The game object to remove.</param>
    public void Remove(GameObject o)
    {
        objects.Remove(o);
    }

    /// <summary>
    /// Clears the GameObjectList, removing all objects it contains.
    /// </summary>
    public void Clear()
    {
        objects = new List<GameObject>();
    }

    /// <summary>
    /// Returns the first game object found with the corrosponding type.
    /// </summary>
    /// <param name="type">The type of game object.</param>
    public GameObject Find(GameObject.ObjectType type)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
            if (objects[i].Type == type)
                return objects[i];
        return null;
    }

    /// <summary>
    /// Returns all game objects found with the corrosponding type.
    /// </summary>
    /// <param name="type">The type of the game object.</param>
    public List<GameObject> FindAll(GameObject.ObjectType type)
    {
        List<GameObject> foundObjects = new List<GameObject>();
        for (int i = objects.Count - 1; i >= 0; i--)
            if (objects[i].Type == type)
                foundObjects.Add(objects[i]);
        return foundObjects;
    }

    /// <summary>
    /// Handles input for all game objects.
    /// </summary>
    public override void HandleInput(Input input)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
            objects[i].HandleInput(input);
    }

    /// <summary>
    /// Updates all game objects.
    /// </summary>
    public override void Update(GameTime gameTime)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
            objects[i].Update(gameTime);
    }

    /// <summary>
    /// Draws all game objects.
    /// </summary>
    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
            objects[i].Draw(spriteBatch, gameTime);
    }

}