using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButtons : MonoBehaviour
{
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
    public void AddReWorkers()
    {
        ServiceLocator.Get<ResourceManager>().Add(ResourceType.Workers, 10);
    }

    public void ToggleFullscreen()
    {
        bool isFullscreen = Screen.fullScreen;

        if (isFullscreen)
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
        }
        else
        {
            Screen.SetResolution(
                Display.main.systemWidth,
                Display.main.systemHeight,
                FullScreenMode.FullScreenWindow
            );
        }
    }
}
