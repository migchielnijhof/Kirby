using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Enemy14 : Enemy
{

    protected const float movementSpeed = 0.22f * Game.SpriteScale;

    public static Texture2D sprite1;

    public static Texture2D sprite2;

    protected byte animationTimer;

    protected byte WalkTimer;

    protected bool seperated;

    public Enemy e;
    
    public Enemy14(GameObject parent) : base(parent, 200, 400, 400, 400)
    {
        currentSprite = sprite1;
        WalkTimer = 8;
        Gravity = 0.04f * Game.SpriteScale;
        spriteSizeOffset.X = -4;
        spriteSizeOffset.Y = -8;
        seperated = false;
    }

    public override void Update(GameTime gameTime)
    {
        if (alive && loaded)
        {
            if (!beingSucked)
            {
                if (!seperated)
                {
                    if (e == null)
                    {
                        e = new Enemy9(parent as Level);
                        e.Position = new Vector2(Position.X, Position.Y - sprite1.Height * Game.SpriteScale);
                        (parent as Level).Add(e);
                        e.Gravity = 0;
                        e.succResistance = true;
                    }
                    else
                        e.Position = new Vector2(Position.X, Position.Y - sprite1.Height * Game.SpriteScale);
                }
                if (onGround)
                {
                    if (WalkTimer == 8)
                    {
                        WalkTimer = 0;
                        if (Velocity.X > 0)
                        {
                            Velocity.X = -movementSpeed;
                            spriteEffect = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            Velocity.X = movementSpeed;
                            spriteEffect = SpriteEffects.None;
                        }
                    }
                    currentSprite = sprite2;
                    Velocity.Y = -3;
                    WalkTimer++;
                    animationTimer = 0;
                }
                else
                {
                    animationTimer++;
                    if (animationTimer == 4)
                    {
                        currentSprite = sprite1;
                    }
                }
            }
            else
            {
                Player p = (parent as Level).Find(ObjectType.Player) as Player;
                if (p.Position.X < Position.X)
                    Velocity.X = 2;
                else
                    Velocity.X = -2;
                seperated = true;
                e.succResistance = false;
                e.Gravity = 0.04f * Game.SpriteScale;
            }
            DoPhysics();
            beingSucked = false;
        }
        else
            CheckForLoad();
    }

}