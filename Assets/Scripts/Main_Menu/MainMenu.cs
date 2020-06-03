using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadSceneAsync(1); // single player game scene
    }

    public void LoadCoopGame()
    {
        SceneManager.LoadSceneAsync(2); // coop game scene
    }
}
