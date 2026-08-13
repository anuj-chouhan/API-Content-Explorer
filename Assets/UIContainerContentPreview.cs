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
        [SerializeField] private CanvasGroup txtDownload;
        [SerializeField] private CanvasGroup txtError;
        [SerializeField] private CanvasGroup txtLoaded;
        [SerializeField] private CanvasGroup txtReady;

        public void SetStatus(ContentPreviewStatus status)
        {
            txtAPIResponseReceived.gameObject.SetActive(true);
            txtDownload.gameObject.SetActive(true);
            txtError.gameObject.SetActive(true);
            txtLoaded.gameObject.SetActive(true);
            txtReady.gameObject.SetActive(true);

            switch (status)
            {
                case ContentPreviewStatus.APIResponseReceived:
                    txtAPIResponseReceived.gameObject.SetActive(false);
                    break;

                case ContentPreviewStatus.Download:
                    txtDownload.gameObject.SetActive(false);
                    break;

                case ContentPreviewStatus.Error:
                    txtError.gameObject.SetActive(false);
                    break;

                case ContentPreviewStatus.Loaded:
                    txtLoaded.gameObject.SetActive(false);
                    break;

                case ContentPreviewStatus.Ready:
                    txtReady.gameObject.SetActive(false);
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

}
