using UnityEngine;

namespace Assets.Math {
    class GraphicsTransforms {
        public static Matrix4x4 ProjectionFromIntrinsicCamera(Matrix4x4 cameraMatrix,
                float projectorWidth, float projectorHeight) {
            float fx = cameraMatrix[0, 0];
            float fy = cameraMatrix[1, 1];
            float cx = cameraMatrix[0, 2];
            float cy = cameraMatrix[1, 2];

            float near = 0.1f;
            float far = 100.0f;

            float w = projectorWidth;
            float h = projectorHeight;

            // fx, fy, cx, cy are in pixels
            // input coordinate system is x left, y up, z forward (right handed)
            // project to view volume where x, y in [-1, 1], z in [0, 1], x right, y up, z forward
            // pre-multiply matrix

            // -(2 * fx / w),           0,   -(2 * cx / w - 1),                           0,
            //             0,  2 * fy / h,      2 * cy / h - 1,                           0,
            //             0,           0,  far / (far - near),  -near * far / (far - near),
            //             0,           0,                   -1,                           0
            Matrix4x4 projectionRightHanded = new Matrix4x4() {
                m00 = 2 * fx / w, m01 = 0, m02 = 1 - 2 * cx / w, m03 = 0,
                m10 = 0, m11 = 2 * fy / h, m12 = 1 - 2 * cy / h, m13 = 0,
                m20 = 0, m21 = 0,          m22 = -(far + near) / (far - near), m23 = -2 * far * near / (far - near),
                m30 = 0, m31 = 0,          m32 = -1, m33 = 0
            };
            return Matrix4x4.Scale(new Vector3(-1, 1, 1)) * projectionRightHanded *
                Matrix4x4.Scale(new Vector3(-1, 1, 1));
        }
    }

    public static class VectorMath {
        public static Vector3 Midpoint(Vector3 v1, Vector3 v2) {
            return v1 + 0.5f * (v2 - v1);
        }
    }
}
