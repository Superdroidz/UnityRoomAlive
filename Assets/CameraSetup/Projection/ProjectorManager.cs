using System.Collections;
using System.Linq;
using Assets.Parsing;
using UnityEngine;

namespace Assets.Projection {
    public class ProjectorManager : MonoBehaviour {

        public static ProjectorCamera Projector { get; private set; }

        void Start() {
            StartCoroutine(FindProjectorData(0));
        }

        void OnDrawGizmos() {
            if (Projector == null || Projector.ProjectedRect == null) return;
            Projector.ProjectedRect.DrawGizmoRect();
        }

        IEnumerator FindProjectorData(int projectorNumber) {
            yield return new WaitUntil(() => EnsembleManager.Manager != null);
            ProjectorData projectorData = EnsembleManager.Manager.data.projectors.ElementAt(projectorNumber);
            Projector = new ProjectorCamera(projectorData);
        }
    }
}
