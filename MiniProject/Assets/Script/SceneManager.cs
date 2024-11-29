using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager class

public class SceneLoader : MonoBehaviour
{
    // Function to load a scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to close the application
    public void QuitApp()
    {
        // If running in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a standalone app
        Application.Quit();
#endif
    }
}
