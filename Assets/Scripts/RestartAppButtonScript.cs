using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartAppButtonScript : MonoBehaviour
{
    public Button restartButton; // Reference to the restart button

    private void Awake()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartApp);
        }
    }

    private void RestartApp()
    {
        // Reload the current scene to restart the app
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("App Restarted");
    }
}
