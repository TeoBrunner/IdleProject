using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingExperienceInfoBlock : LocalizedComponent, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentExpText;
    [SerializeField] private TMP_Text requiredExpText;
    [SerializeField] private Slider progressBar;

    private Building currentBuilding;
    private BuildingExperience buildingExperience;

    private const string LEVEL_KEY = "level";
    private const string MAX_KEY = "max";

    private string levelLabel;
    private string maxLabel;

    protected override void RefreshLocalization()
    {
        levelLabel = Localization.GetString(LEVEL_KEY);
        maxLabel = Localization.GetString(MAX_KEY);

        if (buildingExperience != null)
            UpdateContent();
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<BuildingExperience>(out var experience))
        {
            currentBuilding = building;
            buildingExperience = experience;
            gameObject.SetActive(true);

            RefreshLocalization();
            EventBus.Subscribe<BuildingExperienceChangedEvent>(OnExperienceChanged);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        EventBus.Unsubscribe<BuildingExperienceChangedEvent>(OnExperienceChanged);
        buildingExperience = null;
        currentBuilding = null;
        gameObject.SetActive(false);
    }

    private void OnExperienceChanged(BuildingExperienceChangedEvent e)
    {
        if (e.Building != currentBuilding) return;
        UpdateContent();
    }

    private void UpdateContent()
    {
        if (buildingExperience == null) return;

        int level = buildingExperience.CurrentLevel;
        int current = buildingExperience.CurrentExpInt;
        int required = buildingExperience.RequiredExp;
        bool isMax = buildingExperience.IsMaxLevel;

        if (levelText != null)
        {
            levelText.text = isMax
                ? $"{levelLabel} {level} ({maxLabel})"
                : $"{levelLabel} {level}";
        }

        if (currentExpText != null)
            currentExpText.text = current.ToString();

        if (requiredExpText != null)
            requiredExpText.text = isMax ? "-" : required.ToString();

        if (progressBar != null)
        {
            if (isMax)
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
}