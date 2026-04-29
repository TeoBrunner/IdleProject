using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingExperienceInfoBlock : MonoBehaviour, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentExpText;
    [SerializeField] private TMP_Text requiredExpText;
    [SerializeField] private Slider progressBar;

    private Building currentBuilding;
    private BuildingExperience buildingExperience;
    private LocalizationProvider localizationProvider;

    private const string LEVEL_KEY = "level";
    private const string MAX_KEY = "max";

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
        if (building.TryGetComponent<BuildingExperience>(out var experience))
        {
            currentBuilding = building;
            buildingExperience = experience;
            gameObject.SetActive(true);
            UpdateContent();
            EventBus.Subscribe<BuildingExperienceChangedEvent>(OnExperienceChanged);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        gameObject.SetActive(false);

        if (buildingExperience != null)
        {
            EventBus.Unsubscribe<BuildingExperienceChangedEvent>(OnExperienceChanged);
            buildingExperience = null;
            currentBuilding = null;
        }
    }

    private void OnExperienceChanged(BuildingExperienceChangedEvent e)
    {
        if (e.Building != currentBuilding) return;
        UpdateContent(e);
    }

    private void UpdateContent(BuildingExperienceChangedEvent e = null)
    {
        if (buildingExperience == null) return;

        if(!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        int level = buildingExperience.CurrentLevel;
        int current = buildingExperience.CurrentExpInt;
        int required = buildingExperience.RequiredExp;

        UpdateText();

        if (currentExpText != null)
            currentExpText.text = current.ToString();

        if (requiredExpText != null)
            requiredExpText.text = buildingExperience.IsMaxLevel ? "-" : required.ToString();

        if (progressBar != null)
        {
            if (buildingExperience.IsMaxLevel)
            {
                progressBar.maxValue = 1;
                progressBar.value = 1;
            }
            else
            {
                progressBar.maxValue = required;
                progressBar.value = Mathf.Min(current, required);
            }
        }
    }

    private void UpdateText()
    {
        if (!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        int level = buildingExperience.CurrentLevel;

        if (levelText != null)
        {
            string levelLabel = localizationProvider.GetString(LEVEL_KEY) ?? "Level";
            levelText.text = buildingExperience.IsMaxLevel
                ? $"{levelLabel} {level} ({localizationProvider.GetString(MAX_KEY) ?? "MAX"})"
                : $"{levelLabel} {level}";
        }


    }
}