﻿using System.Collections.Generic;
using Assets.Math;
using UnityEngine;

namespace Assets.Parsing {
    public class ProjectorCamera {

        public Camera CameraComponent { get; private set; }
        public List<Vector3> ProjectedPoints { get; private set; }
        public Vector3 LOSCentre {
            get {
                return ProjectedPoints[0] +
                    0.5f * (ProjectedPoints[2] - ProjectedPoints[0]);
            }
        }

        public ProjectorCamera(Camera cameraObj, ProjectorData projectorData) {
            CameraComponent = cameraObj;

            // Take the inverse because we need to map projector's center to origin.
            Matrix4x4 worldToCameraMatrix = projectorData.pose.inverse;
            // Reflect in z axis because worldToCameraMatrix is OpenGL convention.
            // It uses negative z axis as the direction the camera looks in.
            worldToCameraMatrix = Matrix4x4.Scale(new Vector3(1, 1, -1)) * worldToCameraMatrix;
            // Flip x translation, and y, z rotations to account for left-handedness
            worldToCameraMatrix[0, 3] = -worldToCameraMatrix[0, 3];
            worldToCameraMatrix[0, 1] = -worldToCameraMatrix[0, 1];
            worldToCameraMatrix[1, 0] = -worldToCameraMatrix[1, 0];
            worldToCameraMatrix[0, 2] = -worldToCameraMatrix[0, 2];
            worldToCameraMatrix[2, 0] = -worldToCameraMatrix[2, 0];
            CameraComponent.worldToCameraMatrix = worldToCameraMatrix;

            Matrix4x4 projectionMatrix = GraphicsTransforms.ProjectionFromIntrinsicCamera(
                projectorData.cameraMatrix, projectorData.width, projectorData.height);
            CameraComponent.projectionMatrix = projectionMatrix;

            ProjectedPoints = GetProjectedPoints();
        }

        private List<Vector3> GetProjectedPoints() {
            List<Ray> rays = new List<Ray> {
                Camera.main.ViewportPointToRay(new Vector3(0, 0, 0)),
                Camera.main.ViewportPointToRay(new Vector3(0, 1, 0)),
                Camera.main.ViewportPointToRay(new Vector3(1, 1, 0)),
                Camera.main.ViewportPointToRay(new Vector3(1, 0, 0))
            };

            var projectedPoints = new List<Vector3>();
            foreach (Ray ray in rays) {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, CameraComponent.farClipPlane)) {
                    projectedPoints.Add(hit.point);
                } else {
                    projectedPoints.Add(Vector3.zero);
                }
            }
            return projectedPoints;
        }
    }
}