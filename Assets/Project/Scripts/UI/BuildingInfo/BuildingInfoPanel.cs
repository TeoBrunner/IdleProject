using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    private IBuildingInfoBlock[] infoBlocks;
    private void Awake()
    {
        ServiceLocator.Register<BuildingInfoPanel>(this);
        infoBlocks = GetComponentsInChildren<IBuildingInfoBlock>();
        gameObject.SetActive(false);
    }
    public void Show(Building building)
    {
        foreach (var block in infoBlocks)
        {
            block.OnPanelOpen(building);
        }

        gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
