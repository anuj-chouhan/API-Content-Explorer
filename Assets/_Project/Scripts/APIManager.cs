using GLTFast;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public enum Connections
{
    Connected,
    Disconnected,
    Fetching,
}

public enum ContentStatus
{
    Ready,
    APIResponseReceived,
    Fetching,
    Downloading,
    Loaded,
    Error,
}

public static class APIEvents
{
    public static event System.Action<Connections> OnConnection;
    public static event System.Action<string> OnResponse;
    public static event System.Action<ContentStatus> OnContentStatus;

    public static void Connection(Connections connection)
    {
        OnConnection?.Invoke(connection);
    }

    public static void Response(string responseCode)
    {
        OnResponse?.Invoke(responseCode);
    }

    public static void ContentStatus(ContentStatus contentStatus)
    {
        OnContentStatus?.Invoke(contentStatus);
    }
}


public class APIManager : MonoBehaviour
{
    private static readonly string baseURL =
        "https://pub-d00cc8b8d1824d78a95ef0e94b835d6d.r2.dev";

    public static APIManager Instance;

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

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        APIEvents.Connection(Connections.Fetching);
        APIEvents.ContentStatus(ContentStatus.Ready);

        string url = baseURL + "/Text.txt";

        using (UnityWebRequest request = UnityWebRequest.Head(url))
        {
            yield return request.SendWebRequest();

            APIEvents.Response(((int)request.responseCode).ToString());

            if (request.result == UnityWebRequest.Result.Success)
            {
                APIEvents.Connection(Connections.Connected);
            }
            else
            {
                APIEvents.Connection(Connections.Disconnected);
            }
        }
    }

    public void StopAllFetching()
    {
        StopAllCoroutines();
    }

    public void GetTextFromServer(Action<string> onSuccess, Action onError = null)
    {
        StopAllFetching();

        APIEvents.ContentStatus(ContentStatus.Ready);
        StartCoroutine(DownloadText(onSuccess, onError));
    }

    public void GetImageFromServer(Action<Sprite> onSuccess, Action onError = null)
    {
        StopAllFetching();

        APIEvents.ContentStatus(ContentStatus.Ready);
        StartCoroutine(DownloadImage(onSuccess, onError));
    }

    public void GetGLBModel(Action<GameObject> onSuccess, Action onError = null)
    {
        StopAllFetching();

        APIEvents.ContentStatus(ContentStatus.Ready);
        StartCoroutine(DownloadGLBModelCoroutine(onSuccess, onError));
    }

    public void GetAudio(Action<AudioClip> onSuccess, Action onError = null)
    {
        StopAllFetching();

        APIEvents.ContentStatus(ContentStatus.Ready);
        StartCoroutine(DownloadAudio(onSuccess, onError));
    }

    public string GetVideoURL()
    {
        StopAllFetching();

        string endPoint = "/Video.mp4";
        string url = baseURL + endPoint;

        Debug.Log("The Video URL Is Ready");

        APIEvents.Response("-");
        APIEvents.ContentStatus(ContentStatus.Fetching);

        return url;
    }

    private IEnumerator DownloadText(Action<string> onSuccess, Action onError)
    {
        string endPoint = "/Text.txt";
        string url = baseURL + endPoint;

        APIEvents.ContentStatus(ContentStatus.Fetching);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            // HTTP response has now arrived.
            APIEvents.Response(((int)request.responseCode).ToString());
            APIEvents.ContentStatus(ContentStatus.APIResponseReceived);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Text Download Failed: " + request.error);

                APIEvents.Connection(Connections.Disconnected);
                APIEvents.ContentStatus(ContentStatus.Error);

                onError?.Invoke();
            }
            else
            {
                APIEvents.Connection(Connections.Connected);

                string text = request.downloadHandler.text;

                Debug.Log("Text Downloaded:\n" + text);

                APIEvents.ContentStatus(ContentStatus.Loaded);

                onSuccess?.Invoke(text);
            }
        }
    }

    private IEnumerator DownloadImage(Action<Sprite> onSuccess, Action onError)
    {
        string endPoint = "/Image.png";
        string url = baseURL + endPoint;

        APIEvents.ContentStatus(ContentStatus.Downloading);

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            // HTTP response has now arrived.
            APIEvents.Response(((int)request.responseCode).ToString());
            APIEvents.ContentStatus(ContentStatus.APIResponseReceived);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Image Download Failed: " + request.error);

                APIEvents.Connection(Connections.Disconnected);
                APIEvents.ContentStatus(ContentStatus.Error);

                onError?.Invoke();
            }
            else
            {
                APIEvents.Connection(Connections.Connected);

                Texture2D tex = DownloadHandlerTexture.GetContent(request);

                Sprite fetchedSprite = Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f)
                );

                Debug.Log("Image Loaded Successfully.");

                APIEvents.ContentStatus(ContentStatus.Loaded);

                onSuccess?.Invoke(fetchedSprite);
            }
        }
    }

    private IEnumerator DownloadGLBModelCoroutine(
        Action<GameObject> onSuccess,
        Action onError)
    {
        string endPoint = "/Character.glb";
        string url = baseURL + endPoint;

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("GLTF source URL is empty");

            APIEvents.Connection(Connections.Disconnected);
            APIEvents.ContentStatus(ContentStatus.Error);

            onError?.Invoke();
            yield break;
        }

        APIEvents.Response("-");
        APIEvents.ContentStatus(ContentStatus.Downloading);

        GltfImport gltf = new GltfImport();

        System.Threading.Tasks.Task<bool> loadTask = gltf.Load(url);

        while (!loadTask.IsCompleted)
        {
            yield return null;
        }

        // At this point glTFast has finished loading the remote resource.
        if (!loadTask.Result)
        {
            Debug.LogError("Failed to load GLB data.");

            APIEvents.Connection(Connections.Disconnected);
            APIEvents.ContentStatus(ContentStatus.Error);

            onError?.Invoke();
            yield break;
        }

        APIEvents.Connection(Connections.Connected);
        APIEvents.ContentStatus(ContentStatus.APIResponseReceived);

        GameObject modelGO = new GameObject("GLB_Model");

        System.Threading.Tasks.Task<bool> instTask =
            gltf.InstantiateMainSceneAsync(modelGO.transform);

        while (!instTask.IsCompleted)
        {
            yield return null;
        }

        if (!instTask.Result)
        {
            Debug.LogError("Failed to instantiate GLB scene.");

            APIEvents.ContentStatus(ContentStatus.Error);

            onError?.Invoke();
            yield break;
        }

        Debug.Log("3D Model Is Loaded");

        // Optional: Try to play animation if available.
        UnityEngine.Animation animator =
            modelGO.GetComponent<UnityEngine.Animation>()
            ?? modelGO.GetComponentInChildren<UnityEngine.Animation>();

        if (animator != null && animator.clip != null)
        {
            animator.Play();
        }

        APIEvents.ContentStatus(ContentStatus.Loaded);

        onSuccess?.Invoke(modelGO);
    }

    private IEnumerator DownloadAudio(Action<AudioClip> onSuccess, Action onError)
    {
        string endPoint = "/Audio.mp3";
        string url = baseURL + endPoint;

        APIEvents.ContentStatus(ContentStatus.Downloading);

        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return request.SendWebRequest();

            // HTTP response has now arrived.
            APIEvents.Response(((int)request.responseCode).ToString());
            APIEvents.ContentStatus(ContentStatus.APIResponseReceived);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Audio Download Failed: " + request.error);

                APIEvents.Connection(Connections.Disconnected);
                APIEvents.ContentStatus(ContentStatus.Error);

                onError?.Invoke();
            }
            else
            {
                APIEvents.Connection(Connections.Connected);

                AudioClip fetchedAudio = DownloadHandlerAudioClip.GetContent(request);

                if (fetchedAudio == null)
                {
                    Debug.LogError("Audio Download Failed: AudioClip is null.");

                    APIEvents.ContentStatus(ContentStatus.Error);

                    onError?.Invoke();
                }
                else
                {
                    Debug.Log("Audio Loaded Successfully.");

                    APIEvents.ContentStatus(ContentStatus.Loaded);

                    onSuccess?.Invoke(fetchedAudio);
                }
            }
        }
    }
}