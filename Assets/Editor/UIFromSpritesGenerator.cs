
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIFromSpritesGenerator : EditorWindow
{
    GameObject parentObject;
    List<Sprite> spriteList = new List<Sprite>();
    Vector2 scrollPos;

    [MenuItem("Tools/UI Sprite Importer")]
    public static void ShowWindow()
    {
        GetWindow<UIFromSpritesGenerator>("UI Sprite Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("UI Generator from Sprites", EditorStyles.boldLabel);

        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent GameObject", parentObject, typeof(GameObject), true);

        GUILayout.Space(10);
        GUILayout.Label("Drag & Drop Images/Sprites Below", EditorStyles.boldLabel);
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 100.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop PNG/JPG/Sprites Here");

        HandleDragAndDrop(dropArea);

        GUILayout.Space(10);
        GUILayout.Label("Sprites in Queue", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
        for (int i = 0; i < spriteList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            spriteList[i] = (Sprite)EditorGUILayout.ObjectField(spriteList[i], typeof(Sprite), false);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                spriteList.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("Generate UI Elements"))
        {
            GenerateUIElements();
        }

        if (GUILayout.Button("Reset Tool"))
        {
            ResetTool();
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        if ((evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform) && dropArea.Contains(evt.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (Object dragged in DragAndDrop.objectReferences)
                {
                    string path = AssetDatabase.GetAssetPath(dragged);
                    if (string.IsNullOrEmpty(path)) continue;

                    // Try to load all sprite sub-assets (supports sprite sheets)
                    Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                    bool foundSprite = false;

                    foreach (var sub in subAssets)
                    {
                        if (sub is Sprite sprite && !spriteList.Contains(sprite))
                        {
                            spriteList.Add(sprite);
                            foundSprite = true;
                        }
                    }

                    // Handle single-sprite textures
                    if (!foundSprite)
                    {
                        Object main = AssetDatabase.LoadMainAssetAtPath(path);
                        if (main is Sprite singleSprite && !spriteList.Contains(singleSprite))
                        {
                            spriteList.Add(singleSprite);
                        }
                    }
                }

                evt.Use();
            }
        }
    }

    private void GenerateUIElements()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent GameObject is not assigned.");
            return;
        }

        foreach (Sprite sprite in spriteList)
        {
            if (sprite == null) continue;

            GameObject go = new GameObject(sprite.name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(parentObject.transform, false);

            Image img = go.GetComponent<Image>();
            img.sprite = sprite;
        }

        Debug.Log($"Created {spriteList.Count} UI elements.");
    }

    private void ResetTool()
    {
        parentObject = null;
        spriteList.Clear();
        Debug.Log("Tool has been reset.");
    }
}


//using UnityEngine;
//using UnityEditor;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class UIFromSpritesGenerator : EditorWindow
//{
//    GameObject parentObject;
//    List<Sprite> spriteList = new List<Sprite>();
//    Vector2 scrollPos;

//    [MenuItem("Tools/UI Sprite Importer")]
//    public static void ShowWindow()
//    {
//        GetWindow<UIFromSpritesGenerator>("UI Sprite Importer");
//    }

//    private void OnGUI()
//    {
//        GUILayout.Label("UI Generator from Sprites", EditorStyles.boldLabel);

//        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent GameObject", parentObject, typeof(GameObject), true);

//        GUILayout.Space(10);
//        GUILayout.Label("Drag & Drop Sprites Below", EditorStyles.boldLabel);
//        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 100.0f, GUILayout.ExpandWidth(true));
//        GUI.Box(dropArea, "Drop Sprites Here");

//        HandleDragAndDrop(dropArea);

//        GUILayout.Space(10);
//        GUILayout.Label("Sprites in Queue", EditorStyles.boldLabel);

//        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
//        foreach (var sprite in spriteList)
//        {
//            EditorGUILayout.ObjectField(sprite, typeof(Sprite), false);
//        }
//        EditorGUILayout.EndScrollView();

//        GUILayout.Space(10);

//        if (GUILayout.Button("Generate UI Elements"))
//        {
//            GenerateUIElements();
//        }

//        if (GUILayout.Button("Reset Tool"))
//        {
//            ResetTool();
//        }
//    }

//    private void HandleDragAndDrop(Rect dropArea)
//    {
//        Event evt = Event.current;
//        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
//        {
//            if (!dropArea.Contains(evt.mousePosition))
//                return;

//            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

//            if (evt.type == EventType.DragPerform)
//            {
//                DragAndDrop.AcceptDrag();

//                foreach (Object dragged in DragAndDrop.objectReferences)
//                {
//                    if (dragged is Sprite sprite && !spriteList.Contains(sprite))
//                    {
//                        spriteList.Add(sprite);
//                    }
//                }

//                evt.Use();
//            }
//        }
//    }

//    private void GenerateUIElements()
//    {
//        if (parentObject == null)
//        {
//            Debug.LogError("Parent GameObject is not assigned.");
//            return;
//        }

//        foreach (Sprite sprite in spriteList)
//        {
//            if (sprite == null) continue;

//            GameObject go = new GameObject(sprite.name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
//            go.transform.SetParent(parentObject.transform, false);

//            Image img = go.GetComponent<Image>();
//            img.sprite = sprite;
//        }

//        Debug.Log($"Created {spriteList.Count} UI elements.");
//    }

//    private void ResetTool()
//    {
//        parentObject = null;
//        spriteList.Clear();
//        Debug.Log("Tool has been reset.");
//    }
//}