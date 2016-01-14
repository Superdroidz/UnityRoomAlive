using Windows.Kinect;
using Assets.Interop;
using Assets.Projection;
using Assets.UnityKinect;
using UnityEngine;

namespace Assets.HeadTrack {

    [RequireComponent(typeof(Camera))]
    public class HeadTracker : MonoBehaviour {

        private bool debugMode = false;

        private Camera user;
        private Camera debugCamera;
        private Vector3 headPosition;

        // A default position for the head, which corresponds roughly to
        // someone standing behind the Kinect, when it's placed on a table.
        private Vector3 defaultHeadPosition = new Vector3(0f, 1.1f, -1.4f);

        void Start() {
            headPosition = defaultHeadPosition;
            user = GetComponent<Camera>();
        }

        void Update() {
            if (!debugMode) {
                if (BodySourceManager.Bodies != null) {
                    TrackHead(BodySourceManager.Bodies);
                }

                user.transform.position = headPosition;
                if (ProjectorManager.Projector != null) {
                    user.transform.LookAt(ProjectorManager.Projector.ProjectedRect.Centre);
                } else {
                    user.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
            } else {
                int movementDelta = 1;
                user.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * movementDelta);
                user.transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * movementDelta);
                user.transform.Translate(Vector3.up * Input.GetAxis("Up") * movementDelta);

                int sensitivity = 1;
                user.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * sensitivity, Space.World);
                user.transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * sensitivity, Space.World);
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
