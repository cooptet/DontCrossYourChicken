using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        //Load Main Menu Scene
        SceneManager.LoadScene(0);
    }
}
