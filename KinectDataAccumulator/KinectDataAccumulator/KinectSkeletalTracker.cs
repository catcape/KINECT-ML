using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace KinectDataAccumulator
{

    public class KinectSkeletalTracker
    {
        private BodyFrameReader _bodyFrameReader;
        private readonly Body[] _bodies;

        public KinectSkeletalTracker(KinectSensor sensor)
        {
            if (sensor != null)
            {

                // Initialize the BodyFrameReader and hook up the event handler
                _bodyFrameReader = sensor.BodyFrameSource.OpenReader();
                if (_bodyFrameReader != null)
                {
                    _bodyFrameReader.FrameArrived += BodyFrameReader_FrameArrived;
                }

                // Allocate the array to hold body data (number of bodies the sensor supports)
                _bodies = new Body[sensor.BodyFrameSource.BodyCount];
            }
            else
            {
                throw new Exception("No Kinect _sensor detected.");
            }
        }

        private void BodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    // Refresh the body data
                    bodyFrame.GetAndRefreshBodyData(_bodies);

                    foreach (Body body in _bodies)
                    {
                        if (body != null && body.IsTracked)
                        {
                            // Access the dictionary of joints for this tracked body
                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            foreach (var joint in joints)
                            {
                                JointType jointType = joint.Key;
                                CameraSpacePoint position = joint.Value.Position;
                                Console.WriteLine($"{jointType} Position: X={position.X:F2}, Y={position.Y:F2}, Z={position.Z:F2}");
                            }
                            Console.WriteLine("-------------------------------------------------------------------------------------------");

                            // Create a new dictionary to hold the joint positions in a serializable format
                            var jointPositions = new Dictionary<string, object>();
                            foreach (var joint in joints)
                            {
                                string jointName = joint.Key.ToString();
                                CameraSpacePoint position = joint.Value.Position;
                                jointPositions[jointName] = new { X = position.X, Y = position.Y, Z = position.Z };
                            }

                            // Serialize the joint positions to a JSON string.
                            // Note: Ensure you have added the Newtonsoft.Json package reference.
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(jointPositions, Newtonsoft.Json.Formatting.Indented);
                            Console.WriteLine("JSON output:");
                            Console.WriteLine(json);

                            Console.WriteLine("===========================================================================================");
                        }
                    }
                }
            }
        }

        public void Close()
        {
            // Clean up resources
            if (_bodyFrameReader != null)
            {
                _bodyFrameReader.Dispose();
                _bodyFrameReader = null;
            }
        }
    }

}
