using TMPro;
using UnityEngine;

public class AutoProductionInfoBlock : LocalizedComponent, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text infoText;

    private AutoProducer autoProducer;

    private const string SEC_KEY = "sec";

    private string resourceName;
    private string secText;

    protected override void RefreshLocalization()
    {
        if (autoProducer != null)
        {
            resourceName = Localization.GetString(autoProducer.ProducedResource.ToString().ToLower());
            secText = Localization.GetString(SEC_KEY);
            UpdateContent();
        }
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<AutoProducer>(out var producer))
        {
            autoProducer = producer;
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
        autoProducer = null;
    }

    private void UpdateContent()
    {
        if (autoProducer == null || infoText == null) return;

        float amount = autoProducer.GatherPerAutoClick;
        float interval = autoProducer.AutoClickInterval;

        infoText.text = $"{resourceName}: +{amount} / {interval:F1} {secText}";
    }
}