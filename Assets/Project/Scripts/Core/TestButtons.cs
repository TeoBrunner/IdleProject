using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButtons : MonoBehaviour
{
    public void RestartScene()
    {
        ServiceLocator.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SetLanguage(string language)
    {
        ServiceLocator.Get<LocalizationProvider>().SetLanguage(language);
    }
}
