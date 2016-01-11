using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Parsing {
    public class ProjectorManager : MonoBehaviour {

        public static ProjectorCamera Projector { get; private set; }

        void Start() {
            StartCoroutine(FindProjectorData(0));
        }

        void OnDrawGizmos() {
            if (Projector == null || Projector.ProjectedPoints == null) return;
            for (int i = 0; i < Projector.ProjectedPoints.Count; i++) {
                Gizmos.color = Color.red;
                Vector3 from = Projector.ProjectedPoints[i];
                Vector3 to = (i + 1 != Projector.ProjectedPoints.Count) ?
                                 Projector.ProjectedPoints[i + 1] :
                                 Projector.ProjectedPoints[0];
                Gizmos.DrawLine(@from, to);
            }
        }

        IEnumerator FindProjectorData(int projectorNumber) {
            yield return new WaitUntil(() => EnsembleManager.Data != null);
            ProjectorData data = EnsembleManager.Data.Projectors.ElementAt(projectorNumber);
            Projector = new ProjectorCamera(Camera.main, data);
        }
    }
}
