
using Microsoft.Kinect;

namespace KinectDataAccumulator
{
    class KinectDataCapture
    {
        private ColorFrameReader _colorFrameReader;

        public KinectDataCapture(KinectSensor sensor)
        {
            if (sensor != null)
            {
                _colorFrameReader = sensor.ColorFrameSource.OpenReader();
                _colorFrameReader.FrameArrived += ColorFrameReader_FrameArrived;

                var depthFrameReader = sensor.DepthFrameSource.OpenReader();
                depthFrameReader.FrameArrived += DepthFrameReader_FrameArrived;
            }
        }

        private void ColorFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // Allocate a buffer for the frame data
                    var width = frame.FrameDescription.Width;
                    var height = frame.FrameDescription.Height;
                    byte[] pixels = new byte[width * height * 4]; // assuming BGRA format
                    frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);

                    // Now, you can process or save 'pixels' as needed
                }
            }
        }

        private void DepthFrameReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    FrameDescription frameDesc = depthFrame.FrameDescription;
                    int width = frameDesc.Width;   // typically 512
                    int height = frameDesc.Height; // typically 424

                    // Create an array to hold the depth data.
                    // Each pixel is a ushort (16 bits)
                    ushort[] depthData = new ushort[width * height];

                    // Copy the depth frame data to the array
                    depthFrame.CopyFrameDataToArray(depthData);

                    // Now depthData holds the distance value for each pixel (in millimeters).
                    // You can use this data for processing, visualization, or feeding into a machine learning model.
                }
            }
        }


        public void Close()
        {
            if (_colorFrameReader != null)
            {
                _colorFrameReader.Dispose();
                _colorFrameReader = null;
            }
        }
    }

}
