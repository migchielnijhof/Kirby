using Microsoft.Kinect;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using static Kinect.Kinect;
using kin = Microsoft.Kinect;
/// <summary>
/// Translates input from the kinect for easy use in other classes.
/// </summary>
class Input
{
    Tracker tracker;
    KinectSensor sensor;
    public byte Movement;
    public bool Start, Jump, Crouch, Fly, Succ;

    public Input()
    {
        sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
        if (sensor == null)
        {
            Console.WriteLine("No Kinect sensor found!");
            return;
        }
        tracker = new Tracker(sensor);
        sensor.Start();
    }

    // Updates the input.
    public void Update()
    {
        Movement = 0;
        Start = false;
        Jump = false;
        Crouch = false;
        Fly = false;
        Succ = false;
        if (tracker.skeleton != null)
        {
            kin.Joint j = tracker.skeleton.Joints[kin.JointType.HandRight];
            kin.Joint i = tracker.skeleton.Joints[kin.JointType.HandLeft];
            kin.Joint h = tracker.skeleton.Joints[kin.JointType.Head];
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || h.Position.Y > 0.88)
                Start = true;
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || i.Position.X < -0.60)
                Movement = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || j.Position.X > 0.45)
                Movement = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || j.Position.Y > 0.85 || i.Position.Y > 0.85)
                Fly = true;
            if (Keyboard.GetState().IsKeyDown(Keys.Z) || h.Position.Y > 0.88)
                Jump = true;
            if (Keyboard.GetState().IsKeyDown(Keys.X) || (i.Position.X < -0.60 && j.Position.X > 0.45))
                Succ = true;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || h.Position.Y < 0.75)
                Crouch = true;
        }
        else
        {
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

}