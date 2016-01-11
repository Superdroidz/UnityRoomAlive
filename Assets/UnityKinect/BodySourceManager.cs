using Windows.Kinect;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.UnityKinect {

    public class BodiesEvent : UnityEvent<Body[]> { }

    public class BodySourceManager : MonoBehaviour {

        public static UnityEvent<Body[]> updateBodies = new BodiesEvent();

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
                    Body[] bodies = new Body[_Sensor.BodyFrameSource.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);
                    frame.Dispose();
                    updateBodies.Invoke(bodies);
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
