using UnityEngine;
using System.Collections;

public class ChickenHitFlash : MonoBehaviour
{
    public Renderer[] renderers;
    public Color hitColor = Color.red;
    public float duration = 1.5f;
    public float pulseSpeed = 10f;

    Color[] originalColors;
    Coroutine routine;

    void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StartPulse();
    }

    void StartPulse()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = (Mathf.Sin(t * pulseSpeed) + 1f) * 0.5f;
            for (int i = 0; i < renderers.Length; i++)
                renderers[i].material.color = Color.Lerp(originalColors[i], hitColor, k);
            yield return null;
        }

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
        routine = null;
    }
}