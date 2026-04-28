using TMPro;
using UnityEngine;

public class BuildingHeaderBlock : MonoBehaviour, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    private LocalizationProvider localizationProvider;
    private BuildingIdentity buildingIdentity;
    private void Start()
    {
        localizationProvider = ServiceLocator.Get<LocalizationProvider>();
    }
    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<BuildingIdentity>(out var buildingIdentity))
        {
            gameObject.SetActive(true);

            this.buildingIdentity = buildingIdentity;
            UpdateContent();
            localizationProvider.LocalizationUpdated += UpdateContent;

        }
        else
        {
            Debug.LogWarning("BuildingHeaderBlock: Building does not have a BuildingIdentity component.");
            gameObject.SetActive(false);
        }
    }
    public void OnPanelClose()
    {
        gameObject.SetActive(false);
        if(buildingIdentity != null)
        {
            localizationProvider.LocalizationUpdated -= UpdateContent;
        }
    }
    private void UpdateContent()
    {
        if (localizationProvider == null)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        nameText.text = localizationProvider.GetString(buildingIdentity.NameKey);
        descriptionText.text = localizationProvider.GetString(buildingIdentity.DescriptionKey);
    }
}
