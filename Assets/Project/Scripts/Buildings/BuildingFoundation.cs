using Configs;
using System.Linq;
using UnityEngine;

public class BuildingFoundation : ConfigurableComponent, IInteractable
{
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private GameObject foundationVisual;
    [SerializeField] private GameObject constructionVisual;
    [SerializeField] private FoundationInfoPopup infoPopup;

    private ResourceManager resourceManager;
    private AltarFlameSystem altarFlameSystem;

    private FoundationMainConfig[] foundationConfigs;
    private FoundationMainConfig currentConfig;

    private bool isConstructing;
    private float constructionTimer;
    private float constructionDuration;

    public Building BuildingPrefab => buildingPrefab;
    public float AltarLevelRequired => currentConfig?.AltarLevelRequired ?? 0;
    public float GoldCost => currentConfig?.ConstructionGoldCost ?? 0;
    public float WoodCost => currentConfig?.ConstructionWoodCost ?? 0;
    public float StoneCost => currentConfig?.ConstructionStoneCost ?? 0;
    public float ShardsCost => currentConfig?.ConstructionShardsCost ?? 0;

    private void Start()
    {
        resourceManager = ServiceLocator.Get<ResourceManager>();
        altarFlameSystem = ServiceLocator.Get<AltarFlameSystem>();

        LoadConfigs();

        if (foundationVisual != null)
            foundationVisual.SetActive(true);

        if (constructionVisual != null)
            constructionVisual.SetActive(false);

        if (infoPopup != null)
            infoPopup.gameObject.SetActive(false);
    }

    protected override void LoadConfigs()
    {
        var configsArray = Configs.GetConfigs<FoundationMainConfig>();
        if (configsArray != null)
            foundationConfigs = configsArray.OfType<FoundationMainConfig>().ToArray();

        if (buildingPrefab != null)
        {
            currentConfig = foundationConfigs?.FirstOrDefault(c => c.BuildingID == buildingPrefab.BuildingID);
        }
    }

    private void Update()
    {
        if (!isConstructing) return;

        constructionTimer += Time.deltaTime;
        if (constructionTimer >= constructionDuration)
        {
            CompleteConstruction();
        }
    }

    public void OnPlayerEnter()
    {
        if (isConstructing) return;

        if (infoPopup != null)
        {
            infoPopup.Show(this);
        }
    }

    public void OnPlayerExit()
    {
        if (infoPopup != null)
            infoPopup.Hide();
    }

    public void OnInteract()
    {
        if (isConstructing) return;
        if (!CanBuild()) return;

        StartConstruction();
    }

    public void OnExamine() {}

    public bool CanBuild()
    {
        if (currentConfig == null) return false;

        int altarLevel = altarFlameSystem?.CurrentLevel ?? 0;
        if (altarLevel < currentConfig.AltarLevelRequired) return false;

        if (!resourceManager.HasEnough(ResourceType.Gold, currentConfig.ConstructionGoldCost)) return false;
        if (!resourceManager.HasEnough(ResourceType.Wood, currentConfig.ConstructionWoodCost)) return false;
        if (!resourceManager.HasEnough(ResourceType.Stone, currentConfig.ConstructionStoneCost)) return false;
        if (!resourceManager.HasEnough(ResourceType.Shards, currentConfig.ConstructionShardsCost)) return false;

        return true;
    }

    public void StartConstruction()
    {
        if (!CanBuild() || isConstructing) return;

        resourceManager.TrySpend(ResourceType.Gold, currentConfig.ConstructionGoldCost);
        resourceManager.TrySpend(ResourceType.Wood, currentConfig.ConstructionWoodCost);
        resourceManager.TrySpend(ResourceType.Stone, currentConfig.ConstructionStoneCost);
        resourceManager.TrySpend(ResourceType.Shards, currentConfig.ConstructionShardsCost);

        isConstructing = true;
        constructionDuration = currentConfig.ConstructionTime;
        constructionTimer = 0f;

        if (infoPopup != null)
            infoPopup.gameObject.SetActive(false);

        if (foundationVisual != null)
            foundationVisual.SetActive(false);

        if (constructionVisual != null)
            constructionVisual.SetActive(true);
    }

    private void CompleteConstruction()
    {
        if (buildingPrefab != null)
        {
            Instantiate(buildingPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}