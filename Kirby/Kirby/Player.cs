using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Class of the playable character.
/// </summary>
class Player : AnimatedGameObject
{

    /// <summary>
    /// Current health of the player.
    /// </summary>
    public byte Health;

    /// <summary>
    /// The position of the player.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// The velocity of the player.
    /// </summary>
    public Vector2 Velocity;

    /// <summary>
    /// Maximum health of the player.
    /// </summary>
    const byte maxHealth = 5;

    /// <summary>
    /// The movement speed of a player.
    /// </summary>
    const float movementSpeed = 1.2f;

    /// <summary>
    /// The player's score.
    /// </summary>
    public int score;

    /// <summary>
    /// Whether the player is on the ground or not.
    /// </summary>
    protected bool onGround;

    /// <summary>
    /// The enemy the player has sucked up.
    /// </summary>
    protected GameObject absorbedEnemy;

    /// <summary>
    /// The amount of time the player will remain invulnerable.
    /// </summary>
    protected double invulnerabilityTime;

    /// <summary>
    /// The amount of invulnerabiliyTime the player will get after taking damage.
    /// </summary>
    const double invulnerability = 1.0d;

    /// <summary>
    /// Create a new player.
    /// </summary>
    /// <param name="parent">The game object can communicate with other game objects through the parent.</param>
    public Player(GameObject parent) : base(parent, ObjectType.Player)
    {
        Health = maxHealth;
        score = 0;
        absorbedEnemy = null;
        invulnerabilityTime = 0.0d;
        Velocity = Vector2.Zero;
    }

    public void TakeDamage()
    {
        if (invulnerabilityTime > 0)
            return;
        Health--;
        if (Health == 0)
        {
            Die();
            return;
        }
        invulnerabilityTime = invulnerability;
    }

    protected void Die()
    {

    }

    public override void HandleInput(Input input)
    {
        if (onGround)
        {
            if (input.Movement == 1)
                Velocity.X = movementSpeed;
            else if (input.Movement == 2)
                Velocity.X = -movementSpeed;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {

    }

}