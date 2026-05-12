using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.Hands;
using TMPro; // Required for TextMeshPro


public class HandTrackingCheck : MonoBehaviour
{
    [Tooltip("Drag your TextMeshPro UI element here")]
    [SerializeField] private TMP_Text statusText;

    [Tooltip("How often to check for the connection (in seconds)")]
    [SerializeField] private float checkInterval = 5f;

    void Start()
    {
        if (statusText == null)
        {
            Debug.LogError("HandTrackingValidatorTMP: You forgot to assign the TextMeshPro element in the inspector!");
            return;
        }

        // Start the repeating check
        StartCoroutine(RoutineCheckStatus());
    }

    private IEnumerator RoutineCheckStatus()
    {
        // This while loop runs forever as long as the GameObject is active
        while (true)
        {
            UpdateStatusPanel();

            // Wait for 5 seconds before repeating the loop
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void UpdateStatusPanel()
    {
        // Initialize our message string and add a timestamp so you know it's updating
        string diagnosticMessage = "<b>Hand Tracking Diagnostics</b>\n";
        diagnosticMessage += $"<size=70%>Last check: {System.DateTime.Now.ToString("HH:mm:ss")}</size>\n\n";

        // 1. Check if the specific OpenXR extension is active
        bool isExtensionEnabled = OpenXRRuntime.IsExtensionEnabled("XR_EXT_hand_tracking");

        if (isExtensionEnabled)
        {
            diagnosticMessage += "<color=green>✅ OpenXR Extension:</color> Enabled\n";
        }
        else
        {
            diagnosticMessage += "<color=red>❌ OpenXR Extension:</color> NOT Enabled\n";
        }

        // 2. Check if Unity's XRHandSubsystem is actually running
        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        if (handSubsystems.Count > 0 && handSubsystems[0].running)
        {
            diagnosticMessage += "<color=green>✅ XRHandSubsystem:</color> Running";
        }
        else
        {
            diagnosticMessage += "<color=yellow>⚠️ XRHandSubsystem:</color> Not Running";
        }

        // Push the final message to the UI
        statusText.text = diagnosticMessage;
    }
}