using UnityEngine;
using UnityEngine.Video;

public class ContentController : MonoBehaviour
{
    [SerializeField] private Material matDependencies;
    [SerializeField] private GameObject dynamicModelParent;
    [SerializeField] private VideoPlayer videoPlayer;

    public static ContentController instance;

    private UIPanelExplorer.DataContentDisplay contentDisplayHelper;
    private APIManager apiManager;
    private GameObject currentModel;

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
        contentDisplayHelper = UIPanelExplorer.instance.GetDataContentDisplay;
        apiManager = APIManager.Instance;

        ClearContent();

        videoPlayer.prepareCompleted += VideoPlayerPrepareCompleted;
    }

    private void VideoPlayerPrepareCompleted(VideoPlayer source)
    {
        contentDisplayHelper.HandleLoadingIcon(false);
        contentDisplayHelper.RenderVideo();
    }

    public void LoadText()
    {
        ClearContenHelper();

        contentDisplayHelper.HandleNoContentInfo(false);
        contentDisplayHelper.HandleLoadingIcon(true);

        apiManager.GetTextFromServer(onSuccess: (txt) =>
        {
            contentDisplayHelper.SetText(txt);
            contentDisplayHelper.HandleLoadingIcon(false);
        });
    }

    public void LoadImage()
    {
        ClearContenHelper();

        contentDisplayHelper.HandleNoContentInfo(false);
        contentDisplayHelper.HandleLoadingIcon(true);

        apiManager.GetImageFromServer(onSuccess: (image) =>
        {
            contentDisplayHelper.SetImage(image);
            contentDisplayHelper.HandleLoadingIcon(false);
        });
    }

    public void LoadModel()
    {
        ClearContenHelper();

        contentDisplayHelper.HandleNoContentInfo(false);
        contentDisplayHelper.HandleLoadingIcon(true);

        apiManager.GetGLBModel(onSuccess: (model) =>
        {
            model.transform.position = dynamicModelParent.transform.position;
            model.transform.rotation = dynamicModelParent.transform.rotation;
            model.transform.localScale = dynamicModelParent.transform.localScale;
            model.transform.SetParent(dynamicModelParent.transform);
            currentModel = model;

            contentDisplayHelper.HandleLoadingIcon(false);
        });
    }

    public void LoadVideo()
    {
        ClearContenHelper();

        contentDisplayHelper.HandleNoContentInfo(false);
        contentDisplayHelper.HandleLoadingIcon(true);

        contentDisplayHelper.DisableVideo();

        videoPlayer.url = apiManager.GetVideoURL();
    }

    public void ClearContent()
    {
        ClearContenHelper();
        apiManager.StopAllFetching();
        contentDisplayHelper.HandleNoContentInfo(true);
        contentDisplayHelper.HandleLoadingIcon(false);
    }

    private void ClearContenHelper()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }

        contentDisplayHelper.ClearAll();
    }
}
