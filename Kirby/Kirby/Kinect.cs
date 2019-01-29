using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kin = Microsoft.Kinect;


namespace Kinect
{
    class Kinect
    {
        public kin.KinectSensor sensor;
        
        //kin.Joint hand;

        public void KinectEye()
        {
            Tracker tracker = new Tracker(sensor);
            foreach (var potentialSensor in kin.KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == kin.KinectStatus.Connected)
                {
                    sensor = potentialSensor;

                    if (sensor != null)
                    {
                       
                        this.sensor.Start();
                    }
                }

            }
        }
        internal class Tracker {
            public kin.Skeleton[] skeletons;
            public kin.Skeleton skeleton;
            public Tracker(kin.KinectSensor sensor)
            {
                sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
                sensor.SkeletonStream.Enable();
            }
            void SensorSkeletonFrameReady(object sender, kin.SkeletonFrameReadyEventArgs e)
            {

                using (kin.SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (skeletonFrame != null)
                    {
                        if (this.skeletons == null)
                        {

                            this.skeletons = new kin.Skeleton[skeletonFrame.SkeletonArrayLength];
                        }


                        skeletonFrame.CopySkeletonDataTo(this.skeletons);


                     skeleton = this.skeletons.Where(s => s.TrackingState == kin.SkeletonTrackingState.Tracked).FirstOrDefault();

                        if (skeleton != null)
                        {

                            kin.Joint j = skeleton.Joints[kin.JointType.HandRight];

                            if (j.TrackingState == kin.JointTrackingState.Tracked)
                            {
                                Console.WriteLine("Right Hand: " + j.Position.X + ", " + j.Position.Y + ", " + j.Position.Z);
                            }
                        }
                    }
                }
            }
        }
    }
}
