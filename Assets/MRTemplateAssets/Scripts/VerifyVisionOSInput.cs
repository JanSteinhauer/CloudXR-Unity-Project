using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class VerifyVisionOSInput : MonoBehaviour
{
    [Header("Input Actions")]
    [Tooltip("Bind the Pinch Action from VisionOS_Input here.")]
    public InputActionReference pinchAction;
    
    [Tooltip("Bind the Pointer Pose Action from VisionOS_Input here.")]
    public InputActionReference pointerPoseAction;

    [Header("UI Reference")]
    [Tooltip("Drag a TextMeshPro UI Text component here to display the input data.")]
    public TMP_Text statusText;

    private void OnEnable()
    {
        // Enable the actions so they start listening for input
        if (pinchAction.action != null)
            pinchAction.action.Enable();
            
        if (pointerPoseAction.action != null)
            pointerPoseAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable actions when script is inactive to save performance
        if (pinchAction.action != null)
            pinchAction.action.Disable();
            
        if (pointerPoseAction.action != null)
            pointerPoseAction.action.Disable();
    }

    private void Update()
    {
        if (statusText == null) return;

        // Check the pinch state
        string pinchState = "Action Not Bound";
        if (pinchAction.action != null)
        {
            // IsPressed() returns true if the button/trigger is currently held down
            bool isPinching = pinchAction.action.IsPressed();
            float rawValue = pinchAction.action.ReadValue<float>(); // For trigger buttons this shows 0.0 to 1.0
            pinchState = isPinching ? $"Pressed (Value: {rawValue:F2})" : $"Released (Value: {rawValue:F2})";
        }

        // Check the pointer pose state
        string pointerState = "Action Not Bound";
        if (pointerPoseAction.action != null)
        {
            // Using ReadValueAsObject allows it to safely read whatever Pose/Vector3/Quaternion struct the XR plugin passes
            var poseValue = pointerPoseAction.action.ReadValueAsObject();
            pointerState = poseValue != null ? poseValue.ToString() : "No Tracking Data";
        }

        // Update the TextMeshPro UI
        statusText.text = $"<b>VisionOS Input Verification</b>\n\n" +
                          $"<b>Pinch (Select):</b> {pinchState}\n" +
                          $"<b>Pointer Pose:</b> {pointerState}";
    }
}
