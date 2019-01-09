using Microsoft.Xna.Framework.Input;
/// <summary>
/// Translates input from the kinect for easy use in other classes.
/// </summary>
class Input
{

    public byte Movement;
    public bool Start;

    // Updates the input.
    public void Update()
    {
        Movement = 0;
        Start = false;
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
            Start = true;
    }

}