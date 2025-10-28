using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Quit()
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
