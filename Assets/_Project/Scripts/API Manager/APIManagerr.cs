using GLTFast;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIManagerr : MonoBehaviour
{
    private static readonly string baseURL = "https://raw.githubusercontent.com/anuj-chouhan/Unity-Ar-Assets/main/HostedStuffs";

    public static APIManagerr Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GetTextFromServer(Action<string> onSuccess, Action onError = null)
    {
        StopAllFetching();
        StartCoroutine(DownloadText(onSuccess, onError));
    }

    public void GetImageFromServer(Action<Sprite> onSuccess, Action onError = null)
    {
        StopAllFetching();
        StartCoroutine(DownloadImage(onSuccess, onError));
    }

    public void GetGLBModel(Action<GameObject> onSuccess, Action onError = null)
    {
        StopAllFetching();
        StartCoroutine(DownloadGLBModelCoroutine(onSuccess, onError));
    }

    public string GetVideoURL()
    {
        StopAllFetching();

        string endPoint = "/Video.mp4";
        string url = baseURL + endPoint;

        Debug.Log("The Video Is Loaded");
        return url;
    }

    public void StopAllFetching()
    {
        StopAllCoroutines();
    }

    private IEnumerator DownloadText(Action<string> onSuccess, Action onError)
    {
        string endPoint = "/Text.txt";
        string url = baseURL + endPoint;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Text Download Failed: " + request.error);
                onError?.Invoke();
            }
            else
            {
                string text = request.downloadHandler.text;
                Debug.Log("Text Downloaded:\n" + text);
                onSuccess?.Invoke(text);
            }
        }
    }

    private IEnumerator DownloadImage(Action<Sprite> onSuccess, Action onError)
    {
        string endPoint = "/Image.png";
        string url = baseURL + endPoint;

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Image Download Failed: " + request.error);
                onError?.Invoke();
            }
            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(request);
                Sprite FetchedSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                Debug.Log("Image Loaded Successfully.");
                onSuccess?.Invoke(FetchedSprite);
            }
        }
    }

    private IEnumerator DownloadGLBModelCoroutine(Action<GameObject> onSuccess, Action onError)
    {
        string endPoint = "/Character.glb";
        string url = baseURL + endPoint;

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("GLTF source URL is empty");
            onError?.Invoke();
            yield break;
        }

        GltfImport gltf = new GltfImport();
        System.Threading.Tasks.Task<bool> loadTask = gltf.Load(url);

        while (!loadTask.IsCompleted)
        {
            yield return null;
        }

        if (!loadTask.Result)
        {
            Debug.LogError("Failed to load GLB data.");
            onError?.Invoke();
            yield break;
        }

        GameObject modelGO = new GameObject("GLB_Model");

        System.Threading.Tasks.Task<bool> instTask = gltf.InstantiateMainSceneAsync(modelGO.transform);
        while (!instTask.IsCompleted)
        {
            yield return null;
        }

        if (!instTask.Result)
        {
            Debug.LogError("Failed to instantiate GLB scene.");
            onError?.Invoke();
            yield break;
        }

        Debug.Log("3D Model Is Loaded");

        // Optional: Try to play animation if available
        UnityEngine.Animation animator = modelGO.GetComponent<UnityEngine.Animation>() ?? modelGO.GetComponentInChildren<UnityEngine.Animation>();
        if (animator != null && animator.clip != null)
        {
            animator.Play();
        }

        onSuccess?.Invoke(modelGO);
    }
}
