using UnityEngine;
using UnityEngine.UI;

public class UIContainerCommon : MonoBehaviour
{
    [System.Serializable]
    private class TabButton
    {
        public Button button;

        [SerializeField] private CanvasGroup buttonBackground;

        public void ShowBackground()
        {
            buttonBackground.alpha = 1;
        }

        public void HideBackground()
        {
            buttonBackground.alpha = 0;
        }
    }

    public enum Tabs
    {
        Overview,
        Explorer,
        Technical,
    }

    [SerializeField] private TabButton tabButtonOverview;
    [SerializeField] private TabButton tabButtonExplorer;
    [SerializeField] private TabButton tabButtonTechnical;

    private UICanvasPanelManager panelManager;


    public static UIContainerCommon instance;

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
        panelManager = UICanvasPanelManager.instance;

        tabButtonOverview.button.onClick.AddListener(() =>
        {
            TabSwitcher(Tabs.Overview);
        });

        tabButtonExplorer.button.onClick.AddListener(() =>
        {
            TabSwitcher(Tabs.Explorer);
        });

        tabButtonTechnical.button.onClick.AddListener(() =>
        {
            TabSwitcher(Tabs.Technical);
        });

        TabSwitcher(Tabs.Overview);
    }

    private void TabSwitcher(Tabs tab)
    {
        tabButtonOverview.HideBackground();
        tabButtonExplorer.HideBackground();
        tabButtonTechnical.HideBackground();

        switch (tab)
        {
            case Tabs.Overview:
                panelManager.SetPanelOverview();
                tabButtonOverview.ShowBackground();
                break;

            case Tabs.Explorer:
                panelManager.SetPanelExplorer();
                tabButtonExplorer.ShowBackground();
                break;

            case Tabs.Technical:
                panelManager.SetPanelTechnical();
                tabButtonTechnical.ShowBackground();
                break;
        }
    }

    public void SetTabExplore()
    {
        TabSwitcher(Tabs.Explorer);
    }
}
