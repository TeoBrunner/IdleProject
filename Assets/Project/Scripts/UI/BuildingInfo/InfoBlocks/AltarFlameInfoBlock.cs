using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarFlameInfoBlock : MonoBehaviour, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentFlameText;
    [SerializeField] private TMP_Text requiredFlameText;
    [SerializeField] private Slider progressBar;

    private const string LEVEL_KEY = "level";

    private AltarFlameSystem altarFlameSystem;
    private LocalizationProvider localizationProvider;

    private void Start()
    {
        localizationProvider = ServiceLocator.Get<LocalizationProvider>();
    }
    void OnEnable()
    {
        if (!localizationProvider)
            return;

        UpdateText();
        localizationProvider.LocalizationUpdated += UpdateText;        
    }

    void OnDisable()
    {
        if (!localizationProvider)
            return;

        localizationProvider.LocalizationUpdated -= UpdateText;
    }
    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<AltarFlameSystem>(out var system))
        {
            altarFlameSystem = system;
            gameObject.SetActive(true);
            UpdateContent();
            EventBus.Subscribe<AltarFlameChangedEvent>(OnFlameChanged);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        gameObject.SetActive(false);

        if (altarFlameSystem != null)
        {
            EventBus.Unsubscribe<AltarFlameChangedEvent>(OnFlameChanged);
            altarFlameSystem = null;
        }
    }

    private void OnFlameChanged(AltarFlameChangedEvent e)
    {
        UpdateContent(e);
    }

    private void UpdateContent(AltarFlameChangedEvent e = null)
    {
        if (altarFlameSystem == null) return;

        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        float current = altarFlameSystem.CurrentFlame;
        int required = altarFlameSystem.RequiredFlame;
        int level = altarFlameSystem.CurrentLevel;

        if (levelText != null)
            levelText.text = $"{localizationProvider.GetString(LEVEL_KEY)} {level}";

        if (currentFlameText != null)
            currentFlameText.text = current.ToString();

        if (requiredFlameText != null)
            requiredFlameText.text = required.ToString();

        if (progressBar != null)
        {
            progressBar.maxValue = required;
            progressBar.value = Mathf.Min(current, required);
        }
    }
    private void UpdateText()
    {
        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        int level = altarFlameSystem.CurrentLevel;

        if (levelText != null)
            levelText.text = $"{localizationProvider.GetString("level")} {level}";
    }
}