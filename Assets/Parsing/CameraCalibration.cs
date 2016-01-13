using System;
using UnityEngine;

namespace Assets.Parsing {
    [Serializable]
    public struct CameraCalibration {
        public Matrix4x4 colorCameraMatrix;
        public Matrix4x4 colorLensDistortion;
        public Matrix4x4 depthCameraMatrix;
        public Matrix4x4 depthLensDistortion;
        public Matrix4x4 depthToColorTransform;

        public CameraCalibration(Matrix4x4 colorCameraMatrix,
                                 Matrix4x4 colorLensDistortion,
                                 Matrix4x4 depthCameraMatrix,
                                 Matrix4x4 depthLensDistortion,
                                 Matrix4x4 depthToColorTransform) {
            this.colorCameraMatrix = colorCameraMatrix;
            this.colorLensDistortion = colorLensDistortion;
            this.depthCameraMatrix = depthCameraMatrix;
            this.depthLensDistortion = depthLensDistortion;
            this.depthToColorTransform = depthToColorTransform;
        }
    }
}