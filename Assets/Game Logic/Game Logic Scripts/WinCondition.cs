using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        //ignore if chicken collides with goal point
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("You Won");
            //load win screen scene
            SceneManager.LoadScene(2);
        }
        
    }
}