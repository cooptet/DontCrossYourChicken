using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        //load the Main Level game scene
        SceneManager.LoadScene(1);
    }
}
