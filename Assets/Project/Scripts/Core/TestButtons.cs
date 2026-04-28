using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButtons : MonoBehaviour
{
    [SerializeField] private Building targetBuilding;
    [SerializeField] private int goldPerClick = 10;
    public void RestartScene()
    {
        ServiceLocator.Clear();
        EventBus.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SetLanguage(string language)
    {
        ServiceLocator.Get<LocalizationProvider>().SetLanguage(language);
    }
    public void SimulateBuildingClick()
    {
        if (targetBuilding == null)
        {
            Debug.LogWarning("TestButtons: targetBuilding is not assigned.");
            return;
        }

        EventBus.Publish(new BuildingClickedEvent(targetBuilding));
        Debug.Log($"Simulated click on building: {targetBuilding.name}");
    }

    public void SimulateGoldGain()
    {
        var resourceManager = ServiceLocator.Get<ResourceManager>();
        if (resourceManager == null)
        {
            Debug.LogWarning("TestButtons: ResourceManager not found.");
            return;
        }

        resourceManager.Add(ResourceType.Gold, goldPerClick);
        Debug.Log($"Simulated gold gain: +{goldPerClick}");
    }
}
