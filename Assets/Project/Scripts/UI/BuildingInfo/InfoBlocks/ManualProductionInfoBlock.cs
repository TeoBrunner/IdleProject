using TMPro;
using UnityEngine;

public class ManualProductionInfoBlock : LocalizedComponent, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text infoText;

    private ManualProducer manualProducer;

    private const string INTERACT_KEY = "interact";

    private string resourceName;
    private string interactText;

    protected override void RefreshLocalization()
    {
        if (manualProducer != null)
        {
            resourceName = Localization.GetString(manualProducer.ProducedResource.ToString().ToLower());
            interactText = Localization.GetString(INTERACT_KEY);
            UpdateContent();
        }
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<ManualProducer>(out var producer))
        {
            manualProducer = producer;
            gameObject.SetActive(true);

            RefreshLocalization();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        gameObject.SetActive(false);
        manualProducer = null;
    }

    private void UpdateContent()
    {
        if (manualProducer == null || infoText == null) return;

        float amount = manualProducer.GatherPerClick;
        infoText.text = $"{resourceName}: +{amount} / {interactText}";
    }
}