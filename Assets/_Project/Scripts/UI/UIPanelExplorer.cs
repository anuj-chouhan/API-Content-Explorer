using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelExplorer : MonoBehaviour
{
    [System.Serializable]
    private class ContentPreview
    {
        [SerializeField] private CanvasGroup txtAPIResponseReceived;
        [SerializeField] private CanvasGroup txtDownload;
        [SerializeField] private CanvasGroup txtError;
        [SerializeField] private CanvasGroup txtLoaded;
        [SerializeField] private CanvasGroup txtReady;

        public enum ContentPreviewStatus
        {
            APIResponseReceived,
            Download,
            Error,
            Loaded,
            Ready,
        }

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
    private class ContentPanel
    {
        [SerializeField] private TMP_Dropdown dropdownContentType;
        [SerializeField] private Button buttonFetchContent;
        [SerializeField] private Button buttonClearContent;
        [SerializeField] private TextMeshProUGUI textConnectionStatus;
        [SerializeField] private TextMeshProUGUI textResponseStatus;
    }
}
