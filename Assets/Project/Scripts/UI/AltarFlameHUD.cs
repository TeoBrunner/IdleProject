using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarFlameHUD : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text levelText;

    private const string LEVEL_KEY = "level_short";

    private LocalizationProvider localizationProvider;

    private bool isInitialized;

    private void Start()
    {
        gameObject.SetActive(false);

        EventBus.Subscribe<AltarFlameChangedEvent>(OnFlameChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnFlameChanged);
    }

    private void OnFlameChanged(AltarFlameChangedEvent e)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            gameObject.SetActive(true);
        }

        UpdateDisplay(e.CurrentFlame, e.RequiredFlame, e.Level);
    }

    private void UpdateDisplay(float current, int required, int level)
    {
        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        if (levelText != null)
            levelText.text = $"{localizationProvider.GetString(LEVEL_KEY)} {level}";


        if (progressBar != null)
        {
            progressBar.maxValue = required;
            progressBar.value = Mathf.Min(current, required);
        }
    }
}