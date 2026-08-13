using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIContainerContentPreview : MonoBehaviour
{
    [System.Serializable]
    public class DataContentDisplay
    {
        [SerializeField] private GameObject infoNoContent;
        [SerializeField] private TextMeshProUGUI textText;
        [SerializeField] private Image image;
        [SerializeField] private RawImage videoRenderer;
        [SerializeField] private GameObject loadingIcon;

        public void DisableVideo()
        {
            videoRenderer.gameObject.SetActive(false);
        }

        public void RenderVideo()
        {
            videoRenderer.gameObject.SetActive(true);
        }

        public void SetImage(Sprite sprite)
        {
            image.gameObject.SetActive(true);
            image.sprite = sprite;
        }
        public void SetText(string text)
        {
            textText.gameObject.SetActive(true);
            textText.text = text;
        }

        public void ClearAll()
        {
            textText.text = string.Empty;
            image.sprite = null;

            image.gameObject.SetActive(false);
            textText.gameObject.SetActive(false);
            videoRenderer.gameObject.SetActive(false);
        }

        public void HandleLoadingIcon(bool show)
        {
            loadingIcon.gameObject.SetActive(show);
        }

        public void HandleNoContentInfo(bool show)
        {
            infoNoContent.gameObject.SetActive(show);
        }
    }

    [System.Serializable]
    private class DataContentStatus
    {
        public enum ContentPreviewStatus
        {
            APIResponseReceived,
            Download,
            Error,
            Loaded,
            Ready,
        }

        [SerializeField] private CanvasGroup txtAPIResponseReceived;
        [SerializeField] private CanvasGroup txtFetching;
        [SerializeField] private CanvasGroup txtDownloading;
        [SerializeField] private CanvasGroup txtError;
        [SerializeField] private CanvasGroup txtLoaded;
        [SerializeField] private CanvasGroup txtReady;

        public void Initialize()
        {
            APIEvents.OnContentStatus += APIEvents_OnContentStatus;
        }

        private void APIEvents_OnContentStatus(ContentStatus ContentStatus)
        {
            txtReady.gameObject.SetActive(false);
            txtAPIResponseReceived.gameObject.SetActive(false);
            txtDownloading.gameObject.SetActive(false);
            txtFetching.gameObject.SetActive(false);
            txtLoaded.gameObject.SetActive(false);
            txtError.gameObject.SetActive(false);

            switch (ContentStatus)
            {
                case ContentStatus.Ready:
                    txtReady.gameObject.SetActive(true);
                    break;

                case ContentStatus.APIResponseReceived:
                    txtAPIResponseReceived.gameObject.SetActive(true);
                    break;

                case ContentStatus.Downloading:
                    txtDownloading.gameObject.SetActive(true);
                    break;

                case ContentStatus.Fetching:
                    txtFetching.gameObject.SetActive(true);
                    break;

                case ContentStatus.Loaded:
                    txtLoaded.gameObject.SetActive(true);
                    break;

                case ContentStatus.Error:
                    txtError.gameObject.SetActive(true);
                    break;

            }
        }
    }

    [SerializeField] private DataContentDisplay dataContentDisplay;
    [SerializeField] private DataContentStatus dataContentStatus;

    public DataContentDisplay GetDataContentDisplay => dataContentDisplay;


    public static UIContainerContentPreview instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Singleton Error Here" + transform.name);
        }
    }

    private void Start()
    {
        dataContentStatus.Initialize();
    }

}
