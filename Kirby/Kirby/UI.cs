using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class UI : GameObject
{
    public static Texture2D baseSprite;

    public static Texture2D lifeEmpty;

    public static Texture2D lifeFull;

    public static Texture2D[] numbers = new Texture2D[10];

    protected byte[] livesDigit = new byte[2];

    protected byte[] scoreDigit = new byte[8];

    private Vector2 UIoffset = new Vector2(0, 128);

    protected Player player;

    public UI(GameObject parent) : base(parent, ObjectType.UI)
    {
        player = parent as Player;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(baseSprite, UIoffset * Game.SpriteScale, null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);

        int healthCounter = player.Health;
        for (int x = 0; x < Player.maxHealth; x++) //Player health bar
        {
            if (healthCounter > 0)
            {
                spriteBatch.Draw(lifeFull, new Vector2(((47 + x * (1 + lifeFull.Width) + UIoffset.X) * Game.SpriteScale), ((9 + UIoffset.Y) * Game.SpriteScale)), null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);
                healthCounter--;
            }
            else
                spriteBatch.Draw(lifeEmpty, new Vector2(((47 + x * (1 + lifeFull.Width) + UIoffset.X) * Game.SpriteScale), ((9 + UIoffset.Y) * Game.SpriteScale)), null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);
        }

        for (int x = 0; x < 2; x++) //Player lives
            spriteBatch.Draw(numbers[livesDigit[x]], new Vector2(((129 + x * (1 + numbers[0].Width) + UIoffset.X) * Game.SpriteScale), ((9 + UIoffset.Y) * Game.SpriteScale)), null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);

        for (int x = 0; x < player.score.ToString().Length; x++) //Player score
            spriteBatch.Draw(numbers[scoreDigit[x]], new Vector2(((89 - x * (1 + numbers[0].Width) + UIoffset.X) * Game.SpriteScale), ((1 + UIoffset.Y) * Game.SpriteScale)), null, Color.White, 0, Vector2.Zero, Game.SpriteScale, 0, 0);
    }

    public override void Update(GameTime gameTime)
    {
        livesDigit[0] = (byte)(Game.PlayerLives / 10);
        livesDigit[1] = (byte)(Game.PlayerLives % 10);
        int scoreTemp = player.score;
        for (int x = 0; x < player.score.ToString().Length; x++)
        {
            scoreDigit[x] = (byte)(scoreTemp % 10);
            scoreTemp = scoreTemp / 10;
        }

        base.Update(gameTime);
    }
}