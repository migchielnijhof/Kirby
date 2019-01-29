using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

class WhispyWoods : Boss
{

    protected const float movementSpeed = 0.4f * Game.SpriteScale;

    protected int timer;

    protected int attackTimer;

    protected byte spriteState;

    protected byte attackState;

    protected byte currentHealth;

    protected byte otherAttackChance = 0;

    public static Texture2D sprite1;
    public static Texture2D sprite2;
    public static Texture2D sprite3;
    public static Texture2D sprite4;

    public WhispyWoods(GameObject parent) : base(parent, 200, 3000, 400, 400)
    {
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        currentSprite = sprite1;
        spriteSizeOffset.Y = -49;
        spriteSizeOffset.X = -19;
        Gravity = 0;
        loaded = true;
        timer = 0;
        spriteState = 1;
        attackState = 1;
        attackTimer = 0;
        p.level.bossHealth = 5;
        currentHealth = p.level.bossHealth;
    }

    public override void Update(GameTime gameTime)
    {
        CheckForLoad();
        if (loaded)
        {
            Console.WriteLine(attackState);
            Console.WriteLine("timer: " + timer);
            if (currentHealth != (parent as Level).bossHealth)
            {
                currentHealth = (parent as Level).bossHealth;
                timer = 0;
                attackState = 4;
            }
            timer++;
            switch (attackState)
            {
                case 1:
                    {
                        if (timer == 120)
                        {
                            Console.WriteLine("1");
                            timer = 0;
                            Random random = new Random();
                            int r = random.Next(2);
                            if (r == 1)
                                attackState = 2;
                            else
                                attackState = 3;
                        }
                    }
                    break;
                case 2:
                    {
                        switch (timer)
                        {
                            case 20:
                                currentSprite = sprite2;
                                break;
                            case 25:
                                currentSprite = sprite1;
                                Enemy e = new BossPuff(parent as Level);
                                e.Position = new Vector2(Position.X - 10 * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                                (parent as Level).Add(e);
                                break;
                            case 40:
                                currentSprite = sprite2;
                                break;
                            case 45:
                                currentSprite = sprite1;
                                Enemy e2 = new BossPuff(parent as Level);
                                e2.Position = new Vector2(Position.X - 10 * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                                (parent as Level).Add(e2);
                                break;
                            case 60:
                                currentSprite = sprite2;
                                break;
                            case 65:
                                currentSprite = sprite1;
                                Enemy e3 = new BossPuff(parent as Level);
                                e3.Position = new Vector2(Position.X - 10 * Game.SpriteScale, Position.Y - 15 * Game.SpriteScale);
                                (parent as Level).Add(e3);
                                break;
                            case 80:
                                attackState = 1;
                                timer = 0;
                                break;
                        }
                    }
                    break;
                case 3:
                    {
                        switch (timer)
                        {
                            case 10:
                                Random random = new Random();
                                int direction = random.Next(10);
                                Enemy a = new Apple(parent as Level);
                            a.Position = new Vector2(Position.X - 10 * direction * Game.SpriteScale, Position.Y - 80 * Game.SpriteScale);
                            (parent as Level).Add(a);
                                break;
                            case 100:
                                Random random2 = new Random();
                                int direction2 = random2.Next(10);
                                Enemy a2 = new Apple(parent as Level);
                                a2.Position = new Vector2(Position.X - 10 * direction2 * Game.SpriteScale, Position.Y - 80 * Game.SpriteScale);
                                (parent as Level).Add(a2);
                                break;
                            case 190:
                                Random random3 = new Random();
                                int direction3 = random3.Next(10);
                                Enemy a3 = new Apple(parent as Level);
                                a3.Position = new Vector2(Position.X - 10 * direction3 * Game.SpriteScale, Position.Y - 80 * Game.SpriteScale);
                                (parent as Level).Add(a3);
                                timer = 0;
                                attackState = 1;
                                break;
                        }
                    }
                    break;
                case 4:
                    if (currentHealth != 0)
                    {
                        currentSprite = sprite3;
                        if (timer > 30)
                        {
                            currentSprite = sprite1;
                            timer = 0;
                            attackState = 1;
                        }
                        break;
                    }
                    else
                    {
                        currentSprite = sprite4;
                        break;
                    }

            }
            beingSucked = false;
        }

    }

}