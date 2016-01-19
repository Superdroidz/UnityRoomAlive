using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {
    public Object previousScene;
    public Object nextScene;
    private bool toggle;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Return) && nextScene != null)
        {
            SceneManager.LoadScene(nextScene.name);
        }
	    if (Input.GetKeyDown(KeyCode.Backspace) && previousScene != null) 
        {
            SceneManager.LoadScene(previousScene.name);
        }
	}
}
