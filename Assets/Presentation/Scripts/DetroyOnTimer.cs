using UnityEngine;
using System.Collections;

public class DetroyOnTimer : MonoBehaviour {
    public float destroyAfter;
    float creationTime;

	void Start () {
        creationTime = Time.time;
	}
	
	void Update () {
	    if (creationTime + destroyAfter < Time.time)
        {
            Destroy(gameObject);
        }
	}
}
