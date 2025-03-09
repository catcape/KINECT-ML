using System;
using Microsoft.Kinect;

namespace KinectDataAccumulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize the Kinect _sensor
            KinectSensor sensor = KinectSensor.GetDefault();
            sensor.Open();
            //KinectDataCapture capture = new KinectDataCapture(sensor);
            KinectSkeletalTracker tracker = new KinectSkeletalTracker(sensor);
            while (true)
            {
                Console.WriteLine("Press 'q' to quit");
                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    //capture.Close();
                    tracker.Close();
                    if (sensor != null)
                    {
                        sensor.Close();
                        sensor = null;
                    }
                    break;
                }

            }
        }
    }
}
