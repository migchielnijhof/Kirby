using Microsoft.Xna.Framework.Input;
/// <summary>
/// Translates input from the kinect for easy use in other classes.
/// </summary>
class Input
{

    public byte Movement;
    public bool Start;
    public bool Jump;
    public bool Crouch;

    // Updates the input.
    public void Update()
    {
        Movement = 0;
        Start = false;
        Jump = false;
        Crouch = false;
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
            Start = true;
        if (Keyboard.GetState().IsKeyDown(Keys.A))
            Movement = 2;
        if (Keyboard.GetState().IsKeyDown(Keys.D))
            Movement = 1;
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            Jump = true;
        if (Keyboard.GetState().IsKeyDown(Keys.S))
            Crouch = true;
    }

}