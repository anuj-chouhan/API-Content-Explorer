using UnityEngine;
using UnityEngine.UI;

public class UIPanelOverview : MonoBehaviour
{
    [SerializeField] private Button buttonExplore;

    private UIContainerCommon containerCommon;

    private void Start()
    {
        containerCommon = UIContainerCommon.instance;

        buttonExplore.onClick.AddListener(() =>
        {
            containerCommon.SetTabExplore();
        });
    }
}
