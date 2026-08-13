using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MissingScriptFinder : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts In Scene")]
    static void FindMissingScriptsInScene()
    {
        int goCount = 0;
        int componentsCount = 0;
        int missingCount = 0;

        foreach (GameObject rootGO in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            FindInGO(rootGO, ref goCount, ref componentsCount, ref missingCount);
        }

        Debug.Log($"? Scene scan complete.\nSearched {goCount} GameObjects with {componentsCount} components.\nFound {missingCount} missing scripts.");
    }

    static void FindInGO(GameObject go, ref int goCount, ref int componentsCount, ref int missingCount)
    {
        goCount++;
        Component[] components = go.GetComponents<Component>();

        for (int i = 0; i < components.Length; i++)
        {
            componentsCount++;
            if (components[i] == null)
            {
                missingCount++;
                Debug.LogWarning(
                    $"?? Missing script found on GameObject: <b>{go.name}</b>\nPath: {GetFullPath(go)}",
                    go // ?? this makes it clickable in Console
                );
            }
        }

        foreach (Transform child in go.transform)
        {
            FindInGO(child.gameObject, ref goCount, ref componentsCount, ref missingCount);
        }
    }

    static string GetFullPath(GameObject go)
    {
        string path = go.name;
        Transform current = go.transform.parent;
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
    }
}
