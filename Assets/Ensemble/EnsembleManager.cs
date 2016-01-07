using System;
using System.Collections.Generic;
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
            Matrix4x4 worldToCameraMatrix = projector.pose.inverse;
            worldToCameraMatrix[2, 2] = -worldToCameraMatrix[2, 2];
            //for (int i = 0; i <= 2; i++) {
            //    worldToCameraMatrix[i, 3] /= 10;
            //}
            Debug.Log(worldToCameraMatrix);
            Camera.main.worldToCameraMatrix = worldToCameraMatrix;

            Matrix4x4 projectionMatrix = ProjectionMatrixFromCameraMatrix(projector.cameraMatrix, projector.width, projector.height);
            Debug.Log(projectionMatrix);
            Camera.main.projectionMatrix = projectionMatrix;

            DebugViewCameraCorners();

            //Matrix4x4 rotationMatrix = projector.pose.inverse;
            //Vector4 translation_vec4 = rotationMatrix.GetColumn(3);
            //Vector3 translation = new Vector3(translation_vec4.x, translation_vec4.y, translation_vec4.z);
            //rotationMatrix.SetColumn(3, new Vector4(0, 0, 0, 1));
            //Vector3 scale = Vector3.one;
            //for (int j = 0; j < 3; j++) {
            //    scale[j] = rotationMatrix.GetColumn(j).magnitude;
            //    rotationMatrix.SetColumn(j, rotationMatrix.GetColumn(j) / scale[j]);
            //}
            //Quaternion rotation = QuaternionFromMatrix(rotationMatrix);
            //Camera.main.transform.Translate(translation);
            //Camera.main.transform.rotation = rotation;
            //Camera.main.transform.localScale = scale;
        }

        private void DebugViewCameraCorners() {
            List<Ray> rays = new List<Ray> {
                Camera.main.ViewportPointToRay(new Vector3(0, 0, 0)),
                Camera.main.ViewportPointToRay(new Vector3(1, 0, 0)),
                Camera.main.ViewportPointToRay(new Vector3(1, 1, 0)),
                Camera.main.ViewportPointToRay(new Vector3(0, 1, 0))
            };

            RaycastHit hit;
            foreach (Ray ray in rays) {
                if (Physics.Raycast(ray, out hit, 100)) {
                    Gizmos.DrawCube(hit.transform.position, Vector3.one / 3);
                }
            }
        }

        public static Matrix4x4 ProjectionMatrixFromCameraMatrix(Matrix4x4 cameraMatrix,
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

        public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
            // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
            q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
            q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
            q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
            return q;
        }
    }
}
