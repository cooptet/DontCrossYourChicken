using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //TODO: make sure player gets correct tag
        
        //ignore if chicken collides with goal point
        if (other.gameObject.CompareTag("Player"))
        {
            //TODO: test if this works properly
            Debug.Log("You Won");
            //load win screen scene
            SceneManager.LoadScene(2);
        }
        
    }
}