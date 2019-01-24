using Microsoft.Xna.Framework.Input;
/// <summary>
/// Translates input from the kinect for easy use in other classes.
/// </summary>
class Input
{

    public byte Movement;
    public bool Start, Jump, Crouch, Fly, Succ;

    // Updates the input.
    public void Update()
    {
        Movement = 0;
        Start = false;
        Jump = false;
        Crouch = false;
        Fly = false;
        Succ = false;
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
            Start = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
            Movement = 2;
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
            Movement = 1;
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
            Fly = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Z))
            Jump = true;
        if (Keyboard.GetState().IsKeyDown(Keys.X))
            Succ = true;
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
            Crouch = true;
    }

}