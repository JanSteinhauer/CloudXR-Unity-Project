using UnityEngine;
using UnityEngine.EventSystems;

public class GazeCube : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Leave empty to use the object this script is attached to")]
    public GameObject cube;

    private bool isGazed = false;
    private Renderer cubeRenderer;
    private Color originalColor;

    void Start()
    {
        // Default to the GameObject this script is attached to if nothing is assigned
        if (cube == null) cube = this.gameObject;

        if (cube != null)
        {
            cubeRenderer = cube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                originalColor = cubeRenderer.material.color;
            }
        }
    }

    // This is called automatically by Unity's EventSystem when the pointer (or gaze) enters the object
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnGazeEnter();
    }

    // This is called automatically by Unity's EventSystem when the pointer (or gaze) exits the object
    public void OnPointerExit(PointerEventData eventData)
    {
        OnGazeExit();
    }

    // You can also call this manually if you are using a custom VR/AR gaze system (like MRTK)
    public void OnGazeEnter()
    {
        if (!isGazed && cube != null)
        {
            // Move up by 3 units and change color to Green
            cube.transform.position += new Vector3(0, 3f, 0);
            
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = Color.green;
            }
            
            isGazed = true;
        }
    }

    // You can also call this manually if you are using a custom VR/AR gaze system
    public void OnGazeExit()
    {
        if (isGazed && cube != null)
        {
            // Move back down by 3 units and revert to original color
            cube.transform.position -= new Vector3(0, 3f, 0);
            
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = originalColor;
            }
            
            isGazed = false;
        }
    }
}
