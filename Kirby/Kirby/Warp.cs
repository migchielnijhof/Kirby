using Microsoft.Xna.Framework;

class Warp : GameObject
{

    protected Door door;

    public Warp (GameObject parent, Door door) : base (parent, ObjectType.Warp)
    {
        this.door = door;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        ((parent as Level).Find(ObjectType.Player) as Player).TryDoor(door);
    }
    
}