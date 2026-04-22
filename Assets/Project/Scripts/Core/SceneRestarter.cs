using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    public void RestartScene()
    {
        ServiceLocator.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
