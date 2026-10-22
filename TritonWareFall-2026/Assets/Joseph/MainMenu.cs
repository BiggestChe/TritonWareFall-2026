using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
        GameManager.Instance.StartShift();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
