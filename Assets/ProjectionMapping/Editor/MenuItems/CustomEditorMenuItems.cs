using UnityEngine;
using UnityEditor;

public class CustomEditorMenuItems : EditorWindow{

    [MenuItem("Custom Editor/Add Mesh Colliders %m")]
    private static void PopulateMeshColliders()
    {
        GameObject selection = Selection.activeGameObject;
        AddMeshToObjectOrChildren(selection);
    }

    private static void AddMeshToObjectOrChildren(GameObject selection)
    {
        if (selection.GetComponent<MeshFilter>() != null &&
            selection.GetComponent<Collider>() == null)
        {
            var filter = selection.GetComponent<MeshFilter>().sharedMesh;
            MeshCollider collider = selection.AddComponent<MeshCollider>();
            collider.sharedMesh = filter;
            UnityEngine.Debug.Log("Added MeshCollider to object " + selection.name);
        }
        else
        {
            foreach (Transform child in selection.transform)
            {
                AddMeshToObjectOrChildren(child.gameObject);
            }
        }
    }
}
