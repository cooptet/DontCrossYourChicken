using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthbarController : MonoBehaviour
{
    private int currentHealth;
    public GameObject[] Hearts;

    private void Start()
    {
        currentHealth = Hearts.Length;
    }

    public void ReduceHearts()
    {
        Debug.Log("Lost 1 heart");
        currentHealth -= 1;
        
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i >= currentHealth)
            {
                Hearts[i].gameObject.SetActive(false);
            }
        }
        
        if (currentHealth <= 0)
        {
            //Load GameOver screen
            SceneManager.LoadScene(3);
        }
    }
}
