using System.Collections;
using UnityEngine;

public class FadeAlpha1 : MonoBehaviour
{
    [Tooltip("Leave empty to find 'enviorment' automatically")]
    public GameObject environment;
    
    [Tooltip("Leave empty to find 'Cube' automatically")]
    public GameObject cube;
    
    private Renderer cubeRenderer;

    void Start()
    {
        // Fallback to finding objects by name if they are not assigned in the inspector
        if (environment == null) environment = GameObject.Find("enviorment");
        if (cube == null) cube = GameObject.Find("Cube");

        if (cube != null)
        {
            cubeRenderer = cube.GetComponent<Renderer>();
        }
    }

    // Call this method when the object is clicked (e.g., via a UI Button or Event Trigger)
    public void OnClick()
    {
        ChangeEnvironmentAlpha(1f);
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = Color.green;
        }
    }

    // If the object has a collider, this is called when clicked with the mouse
    private void OnMouseDown()
    {
        OnClick();
    }

    private void ChangeEnvironmentAlpha(float targetAlpha)
    {
        if (environment != null)
        {
            Renderer envRenderer = environment.GetComponent<Renderer>();
            if (envRenderer != null)
            {
                // Using the _Alpha property to match FadeMaterial.cs
                envRenderer.material.SetFloat("_Alpha", targetAlpha);
            }
        }
        else
        {
            Debug.LogWarning("Environment object 'enviorment' not found.");
        }
    }
}
