using UnityEngine;

public class ToggleRendererOnKey : MonoBehaviour {
    public KeyCode key;

    void Update () {
        if (Input.GetKeyDown(key))
        {
            ToggleMeshRenderingRecursively(gameObject);
        }
    }

    void ToggleMeshRenderingRecursively(GameObject obj)
    {
        var meshRend = obj.GetComponent<MeshRenderer>();
        if (meshRend != null)
        {
            meshRend.enabled = !meshRend.enabled;
        }
        else
        {
            foreach (Transform childTr in obj.transform)
            {
                ToggleMeshRenderingRecursively(childTr.gameObject);
            }
        }
    }
}
