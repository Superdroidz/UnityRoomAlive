using UnityEngine;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView_Drawing : MonoBehaviour
{
	public Material BoneMaterial;
	public GameObject BodySourceManager;
	public GameObject target;
	public GameObject paintBrush;

	private BodySourceManager_Drawing _BodyManager;

	GameObject bodyGO;

	private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType> ()
	{
		{ Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
		{ Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
		{ Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
		{ Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

		{ Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
		{ Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
		{ Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
		{ Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

		{ Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
		{ Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
		{ Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
		{ Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

		{ Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.HandRight, Kinect.JointType.WristRight },
		{ Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
		{ Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
		{ Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

		{ Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
		{ Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
		{ Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
		{ Kinect.JointType.Neck, Kinect.JointType.Head },
	};

	void Start ()
	{
		bodyGO = CreateBodyObject ();
		target.SetActive (true);
	}

	void Update ()
	{

		Kinect.Body[] data = BodySourceManager.GetComponent<BodySourceManager_Drawing> ().GetData ();
		if (data == null) return;

		Kinect.Body trackedBody = null;
		foreach (var body in data)
		{
			if (body == null) continue;
			if (body.IsTracked)
			{
				trackedBody = body;
				break;
			}
		}
		if (trackedBody == null) return;

        if (!bodyGO.activeSelf)
        {
            bodyGO.SetActive(true);
        }

		// Now we have found the body we wish to track
		RefreshBodyObject (trackedBody, bodyGO);

		// Update mouse inputs
		checkInputs ();

		Vector3 shoulderR = GetVector3FromJoint (trackedBody.Joints[Kinect.JointType.ShoulderRight]);
		Vector3 handR = GetVector3FromJoint (trackedBody.Joints[Kinect.JointType.HandRight]);

		Vector3 direction = handR - shoulderR;
		RaycastHit hitPoint;

		bool hitSomething = Physics.Raycast (shoulderR, direction, out hitPoint, 10.0f);

		if (!hitSomething)
		{
			Vector3 shoulderL = GetVector3FromJoint (trackedBody.Joints[Kinect.JointType.ShoulderLeft]);
			Vector3 handL = GetVector3FromJoint (trackedBody.Joints[Kinect.JointType.HandLeft]);
			direction = handL - shoulderL;
			hitSomething = Physics.Raycast (shoulderL, direction, out hitPoint, 10.0f);
		}

		if (hitSomething && MBJustToggled[0])
		{
			// insert useful code here
			MBJustToggled[0] = false;
		}

		if (MBJustToggled[2])
		{
			target.SetActive (!target.activeSelf);
			MBJustToggled[2] = false;
		}

		paintBrush.GetComponent<ParticleSystem> ().enableEmission = brushDown;
		if (hitSomething)
		{
			moveGOToHitPoint (paintBrush, hitPoint, 0.05f);
			if (target.activeSelf)
			{
				//target.transform.position = hit.point;
				//target.transform.up = hit.normal;
				//target.transform.Translate (hit.normal * 0.03f, null); // pass null to translate relative to world coords
				moveGOToHitPoint (target, hitPoint, 0.05f);
				//target.transform.Rotate (0, target.GetComponent<SpinObject> ().currentAngle, 0);
			}
		}


	}

	private void moveGOToHitPoint (GameObject obj, RaycastHit hitPoint, float offset)
	{
		obj.transform.position = hitPoint.point;
		obj.transform.up = hitPoint.normal;
		// offset 3cm from wall to avoid z-fighting
		obj.transform.Translate (hitPoint.normal * offset, null); // pass null to translate relative to world coords
	}

	bool[] MBCurrentlyHeld = { false, false, false };
	bool[] MBJustToggled = { false, false, false };
	bool brushDown = false;

	private void checkInputs ()
	{
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButton (i))
			{
				if (!MBCurrentlyHeld[i])
				{
					MBCurrentlyHeld[i] = true;
					MBJustToggled[i] = true;
				}
			}
			else if (MBCurrentlyHeld[i])
			{
				MBCurrentlyHeld[i] = false;
			}
		}

		if (Input.GetKeyDown (KeyCode.Period))
		{
            Debug.Log("Painting");
			brushDown = !brushDown;
		}
	}

	private GameObject CreateBodyObject ()
	{
		GameObject body = new GameObject ("TrackedBody");

		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			GameObject jointObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			jointObj.GetComponent<BoxCollider> ().enabled = false;

			LineRenderer lr = jointObj.AddComponent<LineRenderer> ();
			lr.SetVertexCount (2);
			lr.material = BoneMaterial;
			lr.SetWidth (0.05f, 0.05f);

			jointObj.transform.localScale = new Vector3 (0.03f, 0.03f, 0.03f);
			jointObj.name = jt.ToString ();
			jointObj.transform.parent = body.transform;
		}

        body.SetActive(false);

		return body;
	}

	private void RefreshBodyObject (Kinect.Body body, GameObject bodyObject)
	{
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			Kinect.Joint sourceJoint = body.Joints[jt];
			Kinect.Joint? targetJoint = null;

			if (_BoneMap.ContainsKey (jt))
			{
				targetJoint = body.Joints[_BoneMap[jt]];
			}

			// manipulate joint (i.e. the cube GO)
			Transform jointObj = bodyObject.transform.FindChild (jt.ToString ());
			jointObj.localPosition = GetVector3FromJoint (sourceJoint);

			// manipulate LineRenderer
			LineRenderer lr = jointObj.GetComponent<LineRenderer> ();
			if (targetJoint.HasValue)
			{
				lr.SetPosition (0, jointObj.localPosition);
				lr.SetPosition (1, GetVector3FromJoint (targetJoint.Value));
				lr.SetColors (GetColorForState (sourceJoint.TrackingState), GetColorForState (targetJoint.Value.TrackingState));
			}
			else
			{
				lr.enabled = false;
			}
		}
	}

	private static Color GetColorForState (Kinect.TrackingState state)
	{
		switch (state)
		{
			case Kinect.TrackingState.Tracked:
				return Color.green;

			case Kinect.TrackingState.Inferred:
				return Color.red;

			default:
				return Color.black;
		}
	}

	private static Vector3 GetVector3FromJoint (Kinect.Joint joint)
	{
		// invert X coordinate to conform to Unity coordinate system
		return new Vector3 (-joint.Position.X, joint.Position.Y, joint.Position.Z);
	}
}
