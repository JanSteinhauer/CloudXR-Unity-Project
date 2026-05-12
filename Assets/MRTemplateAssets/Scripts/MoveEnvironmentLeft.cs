using UnityEngine;

public class MoveEnvironmentLeft : MonoBehaviour
{
    [Tooltip("Leave empty to find 'enviorment' automatically")]
    public GameObject environment;
    
    [Tooltip("Leave empty to find 'Cube' automatically")]
    public GameObject cube;

    private Renderer cubeRenderer;

    void Start()
    {
        // Fallback to finding the object by the name you've been using if not assigned in the inspector
        if (environment == null)
        {
            environment = GameObject.Find("enviorment");
        }
        
        if (environment == null)
        {
            Debug.LogWarning("MoveEnvironmentLeft: Could not find 'enviorment' GameObject in the scene.");
        }

        if (cube == null)
        {
            cube = GameObject.Find("Cube");
        }

        if (cube != null)
        {
            cubeRenderer = cube.GetComponent<Renderer>();
        }
    }

    // Call this method when the object is clicked (e.g., via a UI Button or Event Trigger)
    public void OnClick()
    {
        if (environment != null)
        {
            // Move the environment 25 units to the left (along the negative X axis)
            environment.transform.position += new Vector3(-25f, 0, 0);
        }
        else
        {
            Debug.LogWarning("MoveEnvironmentLeft: Environment object is missing when clicked!");
        }

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
}
