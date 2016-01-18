using UnityEngine;

public class EnsureLoaded : MonoBehaviour {
    public GameObject ensureIsLoaded;

	void Awake()
    {
        if (GameObject.Find(ensureIsLoaded.name + "(clone)") == null)
        {
            Instantiate(ensureIsLoaded);
        }
    }
}
