using Windows.Kinect;
using Assets.Interop;
using Assets.Parsing;
using Assets.UnityKinect;
using UnityEngine;

namespace Assets.HeadTrack {

    [RequireComponent(typeof(Camera))]
    public class HeadTracker : MonoBehaviour {

        private bool isTrackingHead = true;
        private Camera user;
        private Vector3 headPosition;

        // A default position for the head, which corresponds roughly to
        // someone standing behind the Kinect, when it's placed on a table.
        private Vector3 defaultHeadPosition = new Vector3(0f, 1.1f, -1.4f);

        void Start() {
            headPosition = defaultHeadPosition;
            if (isTrackingHead) {
                BodySourceManager.updateBodies.AddListener(TrackHead);
            }

            user = GetComponent<Camera>();
        }

        void Update() {
            user.transform.position = headPosition;
            if (ProjectorManager.Projector != null) {
                user.transform.LookAt(ProjectorManager.Projector.LOSCentre);
            } else {
                user.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
        }

        public void TrackHead(Body[] bodies) {
            bool foundTrackedBody = false;
            float distanceToNearest = float.MaxValue;

            foreach (Body body in bodies) {
                if (body.IsTracked) {
                    var cameraSpacePoint = body.Joints[JointType.Head].Position;
                    if (cameraSpacePoint.Z < distanceToNearest) {
                        distanceToNearest = cameraSpacePoint.Z;
                        headPosition = ConvertCameraSpacePoint.ToVector3(cameraSpacePoint);
                        foundTrackedBody = true;
                    }
                }
            }

            if (!foundTrackedBody) {
                headPosition = defaultHeadPosition;
            }
        }
    }
}
