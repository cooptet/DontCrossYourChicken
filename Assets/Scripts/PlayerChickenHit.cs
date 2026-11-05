using UnityEngine;

public class PlayerChickenHit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chicken"))
        {
            Debug.Log("You hit your chicken!");
        }
    }
}
