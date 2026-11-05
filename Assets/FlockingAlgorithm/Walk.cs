using UnityEngine;

public class Walk : MonoBehaviour
{
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float timer = 0f;
    private float targetTime = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= targetTime)
        {
            transform.Rotate(0, 180, 0);
            timer = 0f;
        }

    }
}
