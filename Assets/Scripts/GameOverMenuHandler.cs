using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuHandler : MonoBehaviour
{
    public void BackButtonHandler()
    {
        // Change scene
        SceneManager.LoadSceneAsync("SelectWeapon");
        SceneManager.UnloadSceneAsync("Arena");
    }

    public void ExitButtonHandler()
    {
        Application.Quit();
    }
}
