using System.Collections.Generic;
using Assets.Math;
using UnityEngine;

namespace Assets.Projection {
    public class ProjectionRect {

        public Vector3 Centre {
            get { return VectorMath.Midpoint(ProjectedPoints[0], ProjectedPoints[2]); }
        }

        private Vector3 Normal { get; set; }
        private List<Vector3> ProjectedPoints { get; set; }

        public ProjectionRect(List<Vector3> projectedPoints) {
            ProjectedPoints = projectedPoints;
            var v1 = projectedPoints[1] - projectedPoints[0];
            var v2 = projectedPoints[2] - projectedPoints[0];
            Normal = Vector3.Cross(v1, v2).normalized;
        }

        public void DrawGizmoRect() {
            for (int i = 0; i < ProjectedPoints.Count; i++) {
                Gizmos.color = Color.red;
                Vector3 from = ProjectedPoints[i];
                Vector3 to = i + 1 != ProjectedPoints.Count ?
                             ProjectedPoints[i + 1] :
                             ProjectedPoints[0];
                Gizmos.DrawLine(from, to);
            }
        }

        public override string ToString() {
            string pointStr = "";
            foreach (var point in ProjectedPoints) {
                pointStr += point.ToString() + "\n";
            }
            return pointStr;
        }
    }
}
