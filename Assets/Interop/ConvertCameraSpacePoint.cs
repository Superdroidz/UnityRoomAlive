using Windows.Kinect;
using UnityEngine;

namespace Assets.Interop {
    class ConvertCameraSpacePoint {
        public static Vector3 ToVector3(CameraSpacePoint cameraSpacePoint) {
            return new Vector3(-cameraSpacePoint.X, cameraSpacePoint.Y, cameraSpacePoint.Z);
        }
    }
}
