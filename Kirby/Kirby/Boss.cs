using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

abstract class Boss : Enemy

{
    public static SoundEffect boossDefeatEffect;

    public Boss(GameObject parent, ushort suckScore, ushort starKill, ushort puffKill, ushort pushKill) : base(parent, suckScore, starKill, puffKill, pushKill)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        p.level.UpdateCamera();
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
            BossDefeatParticle bdp = new BossDefeatParticle(parent as Level);
            (parent as Level).Add(bdp);
            if (this is PopoBrosSr)
            {
                bdp.Position = new Vector2(Position.X - 30 * Game.SpriteScale, Position.Y - 40 * Game.SpriteScale);
                MediaPlayer.Play((parent as Level).greenGreens);
            }
            else
            {
                bdp.Position = new Vector2(Position.X - 15 * Game.SpriteScale, Position.Y - 50 * Game.SpriteScale);
                MediaPlayer.Play((parent as Level).sparklingStars);
            }

                boossDefeatEffect.Play();
        }
    }
}