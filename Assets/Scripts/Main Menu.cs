using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        //load the Main Level game scene
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        //Exit the game
        Application.Quit();
    }

    public void OpenTutorial()
    {
        //Load turotial scene
        SceneManager.LoadScene(4);
    }
}
