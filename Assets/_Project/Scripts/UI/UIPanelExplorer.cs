using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
public class DataContentStatus
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

[System.Serializable]
public class DataContentPanel
{
    [SerializeField] private TMP_Dropdown dropdownContentType;
    [SerializeField] private Button buttonFetchContent;
    [SerializeField] private Button buttonClearContent;

    private ContentTypes currentContentType;

    private enum ContentTypes
    {
        Text,
        Image,
        Video,
        Model
    }

    private ContentController contentController;

    public void Intialize()
    {
        contentController = ContentController.instance;
        dropdownContentType.onValueChanged.AddListener(Dropdown);
        buttonFetchContent.onClick.AddListener(FetchContent);
        buttonClearContent.onClick.AddListener(() =>
        {
            contentController.ClearContent();
        });
    }

    private void FetchContent()
    {
        switch (currentContentType)
        {
            case ContentTypes.Text:
                contentController.LoadText();
                break;

            case ContentTypes.Image:
                contentController.LoadImage();
                break;

            case ContentTypes.Video:
                contentController.LoadVideo();
                break;

            case ContentTypes.Model:
                contentController.LoadModel();
                break;
        }
    }

    private void Dropdown(int value)
    {
        switch (value)
        {
            case 0:
                currentContentType = ContentTypes.Text;
                break;

            case 1:
                currentContentType = ContentTypes.Image;
                break;

            case 2:
                currentContentType = ContentTypes.Video;
                break;

            case 3:
                currentContentType = ContentTypes.Model;
                break;

        }
    }
}

[System.Serializable]
public class DataAPIStatus
{
    [SerializeField] private TextMeshProUGUI textConnectionStatus;
    [SerializeField] private TextMeshProUGUI textResponseStatus;
}

public class UIPanelExplorer : MonoBehaviour
{


    [SerializeField] private DataContentDisplay dataContentDisplay;
    [SerializeField] private DataContentStatus dataContentStatus;
    [SerializeField] private DataContentPanel dataContentPanel;
    [SerializeField] private DataAPIStatus dataAPIStatus;

    public DataContentDisplay GetDataContentDisplay => dataContentDisplay;

    public static UIPanelExplorer instance;

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
        dataContentPanel.Intialize();
    }


}
