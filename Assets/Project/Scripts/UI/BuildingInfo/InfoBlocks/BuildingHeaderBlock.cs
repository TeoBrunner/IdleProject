using TMPro;
using UnityEngine;

public class BuildingHeaderBlock : LocalizedComponent, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    private BuildingIdentity buildingIdentity;

    protected override void RefreshLocalization()
    {
        if (buildingIdentity == null) return;

        nameText.text = Localization.GetString(buildingIdentity.NameKey);
        descriptionText.text = Localization.GetString(buildingIdentity.DescriptionKey);
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<BuildingIdentity>(out var identity))
        {
            buildingIdentity = identity;
            gameObject.SetActive(true);

            RefreshLocalization();
        }
        else
        {
            Debug.LogWarning("BuildingHeaderBlock: Building does not have a BuildingIdentity component.");
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        buildingIdentity = null;
        gameObject.SetActive(false);
    }
}