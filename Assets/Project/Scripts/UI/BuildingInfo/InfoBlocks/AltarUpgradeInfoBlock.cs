using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AltarUpgradeInfoBlock : MonoBehaviour, IBuildingInfoBlock
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text buttonText;

    private AltarFlameSystem altarFlameSystem;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
    }
    private void OnDestroy()
    {
        upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
    }
    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<AltarFlameSystem>(out var system))
        {
            altarFlameSystem = system;
            gameObject.SetActive(true);
            UpdateButtonState();
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
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (altarFlameSystem == null) return;

        bool canUpgrade = altarFlameSystem.CanUpgrade;
        upgradeButton.interactable = canUpgrade;
    }

    public void OnUpgradeButtonClicked()
    {
        ServiceLocator.Get<BuildingInfoPanel>()?.Hide();

        altarFlameSystem?.StartUpgrade();
    }
}