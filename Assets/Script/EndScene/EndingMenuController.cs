using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenuController : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}