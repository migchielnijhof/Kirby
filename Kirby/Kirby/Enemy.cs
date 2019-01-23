using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

class Enemy : PhysicsObject
{

    protected const float movementSpeed = 0.6f * Game.SpriteScale;

    const int BoundingBoxSizeX = (int) (16 * Game.SpriteScale);
    const int BoundingBoxSizeY = (int) (16 * Game.SpriteScale);

    public bool beingSucked;

    public bool alive;

    public static Texture2D waddleDeeSprite1;

    public static Texture2D waddleDeeSprite2;

    protected Texture2D currentSprite;

    protected byte animationTimer;

    public Enemy(GameObject parent) : base(parent, ObjectType.Enemy)
    {
        boundingBox.Size = new Point(BoundingBoxSizeX, BoundingBoxSizeY);
        Gravity = 0.1f * Game.SpriteScale;
        beingSucked = false;
        alive = true;
        currentSprite = waddleDeeSprite1;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (!alive)
            return;
        spriteBatch.Draw(currentSprite, Position - (parent as Level).CameraPosition, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, SpriteEffects.FlipHorizontally, 0);
    }
    public override void Update(GameTime gameTime)
    {
        if (animationTimer < 16)
            animationTimer++;
        else
        {
            animationTimer = 0;
        }

        if (!alive)
            return;
        if (!beingSucked)
        {
            Velocity.X = -movementSpeed;
            if (animationTimer == 0)
            {
                if (currentSprite == waddleDeeSprite2)
                    currentSprite = waddleDeeSprite1;
                else
                    currentSprite = waddleDeeSprite2;
            }
        }
        else
        {
            currentSprite = waddleDeeSprite2;
        }
        DoPhysics();
        beingSucked = false;
    }

    protected override bool MapCollisions(TileGrid grid, Vector2 objectMovement)
    {
        if (!alive)
            return false;
        Player p = (parent as Level).Find(ObjectType.Player) as Player;
        if (p.BoundingBox.Intersects(BoundingBox))
            if (beingSucked && p.absorbedEnemy == null)
            {
                p.absorbedEnemy = this;
                alive = false;
                return false;
            }
            else
                p.TakeDamage();
        return base.MapCollisions(grid, objectMovement);
    }

}