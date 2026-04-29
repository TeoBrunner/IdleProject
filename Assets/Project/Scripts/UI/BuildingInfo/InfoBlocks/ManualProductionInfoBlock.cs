using TMPro;
using UnityEngine;

public class ManualProductionInfoBlock : MonoBehaviour, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text infoText;

    private ManualProducer manualProducer;
    private LocalizationProvider localizationProvider;

    private const string INTERACT_KEY = "interact";
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
        if (building.TryGetComponent<ManualProducer>(out var producer))
        {
            manualProducer = producer;
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
        manualProducer = null;
    }

    private void UpdateContent()
    {
        if (manualProducer == null || infoText == null) return;

        if(!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        string resourceName = localizationProvider.GetString(manualProducer.ProducedResource.ToString().ToLower());
        float amount = manualProducer.GatherPerClick;
        string interact = localizationProvider.GetString(INTERACT_KEY);

        infoText.text = $"{resourceName}: +{amount} / {interact}";
    }
}