using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIContainerContentPanel : MonoBehaviour
{
    [System.Serializable]
    private class DataContentPanel
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

        public void Initialize()
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
    private class DataAPIStatus
    {
        [SerializeField] private TextMeshProUGUI textConnectionStatus;
        [SerializeField] private TextMeshProUGUI textResponseStatus;

        public void Initialize()
        {
            APIEvents.OnConnection += APIEvents_OnConnection;
            APIEvents.OnResponse += APIEvents_OnResponse;
        }

        private void APIEvents_OnResponse(string Response)
        {
            textResponseStatus.text = Response;
        }

        private void APIEvents_OnConnection(Connections Connection)
        {
            textConnectionStatus.text = Connection.ToString();
        }
    }


    [SerializeField] private DataContentPanel dataContentPanel;
    [SerializeField] private DataAPIStatus dataAPIStatus;

    private void Start()
    {
        dataContentPanel.Initialize();
        dataAPIStatus.Initialize();
    }
}
