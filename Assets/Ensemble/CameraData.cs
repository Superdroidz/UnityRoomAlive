using UnityEngine;

namespace Ensemble {
    public struct CameraData {
        public CameraCalibration calibration;
        public string hostNameOrAddress;
        public string name;
        public Matrix4x4 pose;
    }
}
