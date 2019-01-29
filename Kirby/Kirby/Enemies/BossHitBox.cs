using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class BossHitBox : Enemy
{

    protected const float movementSpeed = 0;

    public static Texture2D sprite;

    public BossHitBox(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite;
        succResistance = true;
        Gravity = 0;
    }

    public override void TakeHit(bool airPuff)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (!airPuff)
            p.level.bossHealth--;
    }

    public override void Update(GameTime gameTime)
    {
        DoPhysics();

        beingSucked = false;
    }

}