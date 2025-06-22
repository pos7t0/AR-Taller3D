using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Cambia a la escena con el nombre especificado
    public void ChangeScene(string sceneName)
    {
        // Asegúrate de que la escena existe en Build Settings
        SceneManager.LoadScene(sceneName);
    }

    // Cambia a la siguiente escena en el orden de Build Settings
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Recarga la escena actual
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Sale del juego (solo funciona en build, no en el editor)
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}