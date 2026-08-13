using UnityEngine;

public class UICanvasPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject panelOverview;
    [SerializeField] private GameObject panelExplorer;
    [SerializeField] private GameObject panelTechnical;

    public static UICanvasPanelManager instance;

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

    private enum Panels
    {
        Overview,
        Explorer,
        Technical,
    }

    private void PanelManager(Panels panel)
    {
        panelOverview.SetActive(false);
        panelExplorer.SetActive(false);
        panelTechnical.SetActive(false);

        switch (panel)
        {
            case Panels.Overview:
                panelOverview.SetActive(true);
                break;

            case Panels.Explorer:
                panelExplorer.SetActive(true);
                break;

            case Panels.Technical:
                panelTechnical.SetActive(true);
                break;
        }
    }

    public void SetPanelOverview()
    {
        PanelManager(Panels.Overview);
    }

    public void SetPanelExplorer()
    {
        PanelManager(Panels.Explorer);
    }

    public void SetPanelTechnical()
    {
        PanelManager(Panels.Technical);
    }
}
