using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkersInfoBlock : LocalizedComponent, IBuildingInfoBlock
{
    [SerializeField] private TMP_Text workersCountText;
    [SerializeField] private TMP_Text workerProductionInfoText;
    [SerializeField] private TMP_Text totalProductionText;
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;

    private BuildingWorkers buildingWorkers;
    private ResourceManager resourceManager;

    private const string WORKERS_KEY = "workers";
    private const string PER_WORKER_KEY = "per_worker";
    private const string TOTAL_PRODUCTION_KEY = "total_production";
    private const string SEC_KEY = "sec";

    private string workersLabel;
    private string perWorkerLabel;
    private string totalProductionLabel;
    private string secLabel;

    protected override void OnEnable()
    {
        base.OnEnable();
        addButton.onClick.AddListener(OnAddClicked);
        removeButton.onClick.AddListener(OnRemoveClicked);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        addButton.onClick.RemoveListener(OnAddClicked);
        removeButton.onClick.RemoveListener(OnRemoveClicked);
    }
    protected override void RefreshLocalization()
    {
        workersLabel = Localization.GetString(WORKERS_KEY);
        perWorkerLabel = Localization.GetString(PER_WORKER_KEY);
        totalProductionLabel = Localization.GetString(TOTAL_PRODUCTION_KEY);
        secLabel = Localization.GetString(SEC_KEY);

        UpdateAll();
    }

    public void OnPanelOpen(Building building)
    {
        if (building.TryGetComponent<BuildingWorkers>(out var workers))
        {
            buildingWorkers = workers;
            resourceManager = ServiceLocator.Get<ResourceManager>();

            gameObject.SetActive(true);
            buildingWorkers.OnWorkersChanged += UpdateAll;

            RefreshLocalization();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPanelClose()
    {
        if (buildingWorkers != null)
        {
            buildingWorkers.OnWorkersChanged -= UpdateAll;
            buildingWorkers = null;
        }
        gameObject.SetActive(false);
    }

    private void OnAddClicked()
    {
        buildingWorkers?.AddWorker();
    }

    private void OnRemoveClicked()
    {
        buildingWorkers?.RemoveWorker();
    }

    private void UpdateAll()
    {
        UpdateWorkersCount();
        UpdatePerWorkerInfo();
        UpdateTotalProduction();
        UpdateButtons();
    }

    private void UpdateWorkersCount()
    {
        if (buildingWorkers == null || workersCountText == null) return;

        int current = buildingWorkers.CurrentWorkers;
        int max = buildingWorkers.MaxWorkers;
        workersCountText.text = $"{workersLabel}: {current}/{max}";
    }

    private void UpdatePerWorkerInfo()
    {
        if (buildingWorkers == null || workerProductionInfoText == null) return;

        var config = buildingWorkers.CurrentWorkerConfig;
        if (config == null) return;

        string resourceName = Localization.GetString(buildingWorkers.ProducedResourceType.ToString().ToLower());
        float production = config.WorkerProduction;
        float interval = config.WorkerProductionInterval;

        workerProductionInfoText.text = $"{perWorkerLabel}: +{production:F1} {resourceName} / {interval:F1} {secLabel}";
    }

    private void UpdateTotalProduction()
    {
        if (buildingWorkers == null || totalProductionText == null) return;

        var config = buildingWorkers.CurrentWorkerConfig;
        if (config == null || config.WorkerProductionInterval <= 0)
        {
            totalProductionText.text = $"{totalProductionLabel}: 0";
            return;
        }

        int workers = buildingWorkers.CurrentWorkers;
        if (workers == 0)
        {
            totalProductionText.text = $"{totalProductionLabel}: 0";
            return;
        }

        string resourceName = Localization.GetString(buildingWorkers.ProducedResourceType.ToString().ToLower());
        float totalPerInterval = workers * config.WorkerProduction;
        float perSecond = totalPerInterval / config.WorkerProductionInterval;

        totalProductionText.text = $"{totalProductionLabel}: +{perSecond:F1} {resourceName} / {secLabel}";
    }

    private void UpdateButtons()
    {
        if (buildingWorkers == null) return;

        if (addButton != null)
            addButton.interactable = buildingWorkers.CanAddWorker;

        if (removeButton != null)
            removeButton.interactable = buildingWorkers.CanRemoveWorker;
    }
}