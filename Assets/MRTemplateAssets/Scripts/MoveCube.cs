using UnityEngine;

public class MoveCube : MonoBehaviour
{
    [Tooltip("Leave empty to find 'Cube' automatically")]
    public GameObject cube;

    private bool isMovedUp = false;
    private Renderer cubeRenderer;

    void Start()
    {
        if (cube != null)
        {
            cubeRenderer = cube.GetComponent<Renderer>();
        }
    }

    // Call this method when the object is clicked (e.g., via a UI Button or Event Trigger)
    public void OnClick()
    {
        Debug.Log("MoveCube OnClick was triggered!");
        
        if (cube == null) 
        {
            Debug.LogError("MoveCube: The 'cube' reference is missing!");
            return;
        }

        if (isMovedUp)
        {
            // Move up by 3 units and change color to green
            cube.transform.position += new Vector3(0, 3f, 0);
            
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = Color.green;
            }
            
            isMovedUp = false;
        }
        else
        {
            // Move down by 3 units and change color to red
            cube.transform.position -= new Vector3(0, 7f, 0);
            
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = Color.red;
            }
            
            isMovedUp = true;
        }
    }

    // If the object has a collider, this is called when clicked with the mouse
    private void OnMouseDown()
    {
        OnClick();
    }
}
