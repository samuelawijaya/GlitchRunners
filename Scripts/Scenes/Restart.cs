using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart()
    {
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        // If you’re testing in the Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If this is a built (standalone/WebGL) game
            Application.Quit();
#endif
    }
}