using System.Linq;
using UnityEngine;

namespace Ensemble {
    public class EnsembleManager : MonoBehaviour {

        public Camera cameraPrefab;
        public string ensembleFolder;
        public EnsembleData Data { get; private set; }

        // Use this for initialization
        void Start() {
            string ensembleFilePath = Application.dataPath +
                "/Calibrations/" + ensembleFolder + "/calibration.xml";
            Data = new EnsembleData(ensembleFilePath);

            // Set this to choose which projector this Unity instance uses.
            int localProjectorNumber = 0;
            ProjectorData projector = Data.projectors.ElementAt(localProjectorNumber);

            Camera cameraInstance = Instantiate(cameraPrefab);
            cameraInstance.worldToCameraMatrix = projector.pose.inverse;
            cameraInstance.projectionMatrix =
                ProjectionMatrixFromCameraMatrix(projector.cameraMatrix, projector.width, projector.height);
            cameraInstance.tag = "MainCamera";
        }

        Matrix4x4 ProjectionMatrixFromCameraMatrix(Matrix4x4 cameraMatrix,
                float projectorWidth, float projectorHeight) {
            float fx = cameraMatrix[0, 0];
            float fy = cameraMatrix[1, 1];
            float cx = cameraMatrix[0, 2];
            float cy = cameraMatrix[1, 2];

            float near = 0.1f;
            float far = 100.0f;

            float w = projectorWidth;
            float h = projectorHeight;

            return new Matrix4x4() {
                m00 = -(2 * fx / w), m01 = 0, m02 = -(2 * cx / w - 1), m03 = 0,
                m10 = 0, m11 = 2 * fy / h, m12 = 2 * cy / h - 1, m13 = 0,
                m20 = 0, m21 = 0, m22 = far / (far - near), m23 = -near * far / (far - near),
                m30 = 0, m31 = 0, m32 = 1, m33 = 0
            };
        }
    }
}
