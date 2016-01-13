using Windows.Kinect;
using UnityEngine;

namespace Assets.UnityKinect {

    public class BodySourceManager : MonoBehaviour {

        public static Body[] Bodies { get; private set; }
        private KinectSensor _Sensor;
        private BodyFrameReader _Reader;

        void Start() {
            _Sensor = KinectSensor.GetDefault();

            if (_Sensor != null) {
                _Reader = _Sensor.BodyFrameSource.OpenReader();

                if (!_Sensor.IsOpen) {
                    _Sensor.Open();
                }
            }
        }

        void Update() {
            if (_Reader != null) {
                var frame = _Reader.AcquireLatestFrame();
                if (frame != null) {
                    Bodies = new Body[_Sensor.BodyFrameSource.BodyCount];
                    frame.GetAndRefreshBodyData(Bodies);
                    frame.Dispose();
                }
            }
        }

        void OnApplicationQuit() {
            if (_Reader != null) {
                _Reader.Dispose();
                _Reader = null;
            }

            if (_Sensor != null) {
                if (_Sensor.IsOpen) {
                    _Sensor.Close();
                }

                _Sensor = null;
            }
        }
    }
}
