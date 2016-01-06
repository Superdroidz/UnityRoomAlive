using System.Linq;
using UnityEngine;

namespace Ensemble {
    public class EnsembleManager : MonoBehaviour {

        public string ensembleFolder;
        public EnsembleData Data { get; private set; }

        // Use this for initialization
        void Start() {
            string ensembleFilePath = Application.dataPath +
                "/Calibrations/" + ensembleFolder + "/calibration.xml";
            Data = new EnsembleData(ensembleFilePath);

            // Set this to choose which projector this Unity instance uses.
            int localProjectorNumber = 0;
            SetCameraFromProjector(Data.projectors.ElementAt(localProjectorNumber));
        }

        void SetCameraFromProjector(ProjectorData projector) {
            // Scale z by -1 because Unity uses OpenGL convention where Camera's forward
            // direction is negative.
            Matrix4x4 worldToCameraMatrix = Matrix4x4.Scale(new Vector3(1, 1, -1)) * projector.pose.inverse;
            // Scale down translations by 10 (seems to help in the case of 219b's calibration)
            for (int i = 0; i <= 2; i++) {
                worldToCameraMatrix[i, 3] = worldToCameraMatrix[i, 3] / 10;
            }
            Debug.Log(worldToCameraMatrix);
            Camera.main.worldToCameraMatrix = worldToCameraMatrix;
            Camera.main.projectionMatrix =
                ProjectionMatrixFromCameraMatrix(projector.cameraMatrix, projector.width, projector.height);

            Camera.main.tag = "MainCamera";
        }

        Matrix4x4 ProjectionMatrixFromCameraMatrix(Matrix4x4 cameraMatrix,
                float projectorWidth, float projectorHeight) {
            float fx = cameraMatrix[0, 0];
            float fy = cameraMatrix[1, 1];
            float cx = cameraMatrix[0, 2];
            float cy = cameraMatrix[1, 2];

            float near = 0.3f;
            float far = 1000.0f;

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
