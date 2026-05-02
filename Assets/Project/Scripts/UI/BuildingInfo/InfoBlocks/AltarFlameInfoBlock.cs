using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarFlameInfoBlock : LocalizedComponent, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text currentFlameText;
    [SerializeField] private TMP_Text requiredFlameText;
    [SerializeField] private Slider progressBar;

    private AltarFlameSystem altarFlameSystem;

    private const string LEVEL_KEY = "level";

    private string levelLabel;

    protected override void RefreshLocalization()
    {
        levelLabel = Localization.GetString(LEVEL_KEY);

        if (altarFlameSystem != null)
            UpdateContent();
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<AltarFlameSystem>(out var system))
        {
            altarFlameSystem = system;
            gameObject.SetActive(true);

            RefreshLocalization();
            EventBus.Subscribe<AltarFlameChangedEvent>(OnFlameChanged);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        EventBus.Unsubscribe<AltarFlameChangedEvent>(OnFlameChanged);
        altarFlameSystem = null;
        gameObject.SetActive(false);
    }

    private void OnFlameChanged(AltarFlameChangedEvent e)
    {
        UpdateContent();
    }

    private void UpdateContent()
    {
        if (altarFlameSystem == null) return;

        int level = altarFlameSystem.CurrentLevel;
        float current = altarFlameSystem.CurrentFlame;
        int required = altarFlameSystem.RequiredFlame;

        if (levelText != null)
            levelText.text = $"{levelLabel} {level}";

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
}