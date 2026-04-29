using TMPro;
using UnityEngine;

public class AutoProductionInfoBlock : MonoBehaviour, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text infoText;

    private AutoProducer autoProducer;
    private LocalizationProvider localizationProvider;

    private const string SEC_KEY = "sec";
    private void Start()
    {
        localizationProvider = ServiceLocator.Get<LocalizationProvider>();
    }
    void OnEnable()
    {
        if (!localizationProvider)
            return;

        UpdateContent();
        localizationProvider.LocalizationUpdated += UpdateContent;
    }

    void OnDisable()
    {
        if (!localizationProvider)
            return;

        localizationProvider.LocalizationUpdated -= UpdateContent;
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<AutoProducer>(out var producer))
        {
            autoProducer = producer;
            gameObject.SetActive(true);
            UpdateContent();
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

        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        string resourceName = localizationProvider.GetString(autoProducer.ProducedResource.ToString().ToLower());
        float amount = autoProducer.GatherPerAutoClick;
        float interval = autoProducer.AutoClickInterval;
        string sec = localizationProvider.GetString(SEC_KEY);

        infoText.text = $"{resourceName}: +{amount} / {interval:F1} {sec}";
    }
}