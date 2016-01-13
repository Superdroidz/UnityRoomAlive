using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {
    public Object previousScene;
    public Object nextScene;

	void Update () {
	    if (Input.GetKeyDown(KeyCode.P) && previousScene != null)
        {
            SceneManager.LoadScene(previousScene.name);
        }
        if (Input.GetKeyDown(KeyCode.N) && nextScene != null)
        {
            SceneManager.LoadScene(nextScene.name);
        }
	}
}
