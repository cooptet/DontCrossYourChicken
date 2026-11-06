using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void LoadMainMenu()
    {
        //Load Main Menu Scene
        SceneManager.LoadScene(0);
    }
}
