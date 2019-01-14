using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class UI : GameObject
{



    public UI(GameObject parent) : base(parent, ObjectType.UI)
    {

    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        base.Draw(spriteBatch, gameTime);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}