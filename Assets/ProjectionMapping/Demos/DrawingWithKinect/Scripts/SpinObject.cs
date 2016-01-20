using UnityEngine;
using System.Collections;

public class SpinObject : MonoBehaviour {

	public float speed = 1f;

	public float currentAngle = 0f;
	
	void Update () {
		currentAngle += speed;
	}
}
