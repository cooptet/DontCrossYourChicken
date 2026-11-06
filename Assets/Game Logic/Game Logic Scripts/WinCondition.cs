using System;
using UnityEngine;
public class WinCondition : MonoBehaviour
{
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO: make sure player gets correct tag
        
        //ignore if chicken collides with goal point
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("You Won");
            //TODO: trigger win state: Open window with text & option to return to menu
        }
        
    }
}