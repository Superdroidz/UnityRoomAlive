using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
