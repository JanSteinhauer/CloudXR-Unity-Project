using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.EventSystems;

public class DisplaySpatialAnchors : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Reference")]
    [Tooltip("Drag your TextMeshPro UI element here.")]
    public TMP_Text displayText;

    private FirebaseFirestore db;
    private bool isFirebaseInitialized = false;

    void Start()
    {
        // Set initial text
        if (displayText != null)
        {
            displayText.text = "Waiting for interaction...";
        }

        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to the Firebase Firestore instance
                db = FirebaseFirestore.DefaultInstance;
                isFirebaseInitialized = true;
                Debug.Log("Firebase Initialized Successfully.");
                FetchAndDisplayAnchors();
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                if (displayText != null) displayText.text = "Firebase Init Failed";
            }
        });
    }

    /// <summary>
    /// Fetches the documents from the "spatial_anchors" collection and updates the TextMeshPro.
    /// You can call this method from a UnityEvent (e.g., an XR Simple Interactable or a UI Button).
    /// </summary>
    public void FetchAndDisplayAnchors()
    {
        if (!isFirebaseInitialized || db == null)
        {
            Debug.LogWarning("Firebase is not initialized yet.");
            if (displayText != null) displayText.text = "Firebase not ready.";
            return;
        }

        if (displayText != null) displayText.text = "Loading spatial anchors...";

        // Reference the "spatial_anchors" collection as seen in your screenshot
        CollectionReference anchorsRef = db.Collection("spatial_anchors");
        
        // Fetch the documents asynchronously
        anchorsRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error fetching spatial anchors.");
                if (displayText != null) displayText.text = "Error fetching data.";
                return;
            }

            QuerySnapshot snapshot = task.Result;
            string resultText = "<b>Spatial Anchors:</b>\n\n";
            int count = 0;

            // Iterate through the documents and extract their IDs
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                resultText += $"- {document.Id}\n";
                count++;
            }

            if (count == 0)
            {
                resultText += "No anchors found.";
            }

            // Update the UI
            if (displayText != null)
            {
                displayText.text = resultText;
            }
            
            Debug.Log($"Successfully fetched {count} spatial anchors.");
        });
    }

    /// <summary>
    /// This automatically fires if the GameObject this is attached to has a Collider 
    /// and is clicked using a Physics Raycaster (or an EventSystem).
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        FetchAndDisplayAnchors();
    }
}
