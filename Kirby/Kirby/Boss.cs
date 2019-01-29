using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

abstract class Boss : Enemy
    
{
    public Boss(GameObject parent, ushort suckScore, ushort starKill, ushort puffKill, ushort pushKill) : base(parent, suckScore, starKill, puffKill, pushKill)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        p.level.UpdateCamera();
        p.level.cameraLocked = true;
        succResistance = true;
    }

    public override void TakeHit(bool airPuff)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (!airPuff)
            p.level.bossHealth--;
        if (p.level.bossHealth == 0)
        {
            alive = false;
            p.score += StarKill;
            p.level.cameraLocked = false;
        }
    }
}