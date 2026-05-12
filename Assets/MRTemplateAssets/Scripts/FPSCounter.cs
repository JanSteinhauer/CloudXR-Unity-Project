using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [Tooltip("Assign the TextMeshPro component. If empty, it will look for one on this GameObject.")]
    public TMP_Text fpsText;

    [Tooltip("How often the FPS text updates (in seconds) to prevent rapid flickering.")]
    public float updateInterval = 0.5f;

    private int frames = 0;
    private float timeLeft;

    void Start()
    {
        // Try to find a TMP_Text component on this object if one isn't assigned
        if (fpsText == null)
        {
            fpsText = GetComponent<TMP_Text>();
        }
        
        if (fpsText == null)
        {
            Debug.LogWarning("FPSCounter: No TMP_Text component assigned or found on this object. Please assign one.");
        }

        timeLeft = updateInterval;
    }

    void Update()
    {
        if (fpsText == null) return;

        // Count frames and subtract time
        timeLeft -= Time.unscaledDeltaTime;
        frames++;

        // Once the interval has passed, update the text and reset
        if (timeLeft <= 0.0f)
        {
            // Calculate frames per second
            float fps = frames / updateInterval;
            
            // Update the UI text
            fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";

            // Reset timers for the next interval
            timeLeft = updateInterval;
            frames = 0;
        }
    }
}
