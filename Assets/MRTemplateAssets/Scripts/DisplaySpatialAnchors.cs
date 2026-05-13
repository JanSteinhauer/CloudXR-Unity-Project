using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class DisplaySpatialAnchors : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("Drag your TextMeshPro UI element here.")]
    public TMP_Text displayText;

    [Header("Anchor Spawning")]
    [Tooltip("Optional: Drag a prefab here. If left empty, it will spawn standard Unity Cubes.")]
    public GameObject anchorPrefab;

    private FirebaseFirestore db;
    private ListenerRegistration listenerRegistration;
    
    // Keep track of spawned cubes so we can update them if they move in the database
    private Dictionary<string, GameObject> spawnedAnchors = new Dictionary<string, GameObject>();

    void Start()
    {
        if (displayText != null) displayText.text = "Initializing Firebase...";

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase Initialized Successfully.");
                StartListeningToAnchors(); // Start listening automatically
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                if (displayText != null) displayText.text = "Firebase Init Failed";
            }
        });
    }

    private void StartListeningToAnchors()
    {
        if (displayText != null) displayText.text = "Listening for new spatial anchors in real-time...";

        // Listen for real-time updates to the spatial_anchors collection
        listenerRegistration = db.Collection("spatial_anchors").Listen(snapshot =>
        {
            if (displayText != null) 
                displayText.text = $"Tracking {snapshot.Count} active anchors.";

            // Process every change (Added, Modified, Removed)
            foreach (DocumentChange change in snapshot.GetChanges())
            {
                if (change.ChangeType == DocumentChange.Type.Added || change.ChangeType == DocumentChange.Type.Modified)
                {
                    UpdateOrSpawnAnchor(change.Document);
                }
                else if (change.ChangeType == DocumentChange.Type.Removed)
                {
                    RemoveAnchor(change.Document.Id);
                }
            }
        });
    }

    private void UpdateOrSpawnAnchor(DocumentSnapshot document)
    {
        if (!document.Exists) return;

        try
        {
            Dictionary<string, object> data = document.ToDictionary();
            
            // Validate that we have position data
            if (!data.ContainsKey("position")) return;

            // 1. Parse Position
            Dictionary<string, object> posMap = data["position"] as Dictionary<string, object>;
            float px = float.Parse(posMap["x"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            float py = float.Parse(posMap["y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            float pz = float.Parse(posMap["z"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            Vector3 position = new Vector3(px, py, pz);

            // 2. Parse Rotation
            Quaternion rotation = Quaternion.identity;
            if (data.ContainsKey("rotation"))
            {
                Dictionary<string, object> rotMap = data["rotation"] as Dictionary<string, object>;
                float rw = float.Parse(rotMap["w"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                float rx = float.Parse(rotMap["x"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                float ry = float.Parse(rotMap["y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                float rz = float.Parse(rotMap["z"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                rotation = new Quaternion(rx, ry, rz, rw);
            }

            // 3. Parse Scale
            Vector3 scale = Vector3.one;
            if (data.ContainsKey("scale"))
            {
                Dictionary<string, object> scaleMap = data["scale"] as Dictionary<string, object>;
                float sx = float.Parse(scaleMap["x"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                float sy = float.Parse(scaleMap["y"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                float sz = float.Parse(scaleMap["z"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                scale = new Vector3(sx, sy, sz);
            }

            // 4. Check if we need to spawn or just update
            if (spawnedAnchors.ContainsKey(document.Id))
            {
                // Anchor exists, just update its transform
                GameObject existingObj = spawnedAnchors[document.Id];
                existingObj.transform.position = position;
                existingObj.transform.rotation = rotation;
                existingObj.transform.localScale = scale;
            }
            else
            {
                // Anchor is new, spawn it
                GameObject newObj;
                if (anchorPrefab != null)
                {
                    newObj = Instantiate(anchorPrefab, position, rotation);
                }
                else
                {
                    // Create a simple Unity Cube if no prefab is provided
                    newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newObj.transform.position = position;
                    newObj.transform.rotation = rotation;
                    
                    // Optional: remove collision if you don't want them physically interacting with everything
                    Destroy(newObj.GetComponent<BoxCollider>()); 
                }
                
                newObj.transform.localScale = scale;
                newObj.name = $"SpatialAnchor_{document.Id}";
                
                // Track it so we don't spawn duplicates
                spawnedAnchors[document.Id] = newObj;
                Debug.Log($"Spawned new anchor at {position}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to parse anchor {document.Id}: {ex.Message}");
        }
    }

    private void RemoveAnchor(string id)
    {
        if (spawnedAnchors.ContainsKey(id))
        {
            Destroy(spawnedAnchors[id]);
            spawnedAnchors.Remove(id);
            Debug.Log($"Removed anchor {id}");
        }
    }

    void OnDestroy()
    {
        // Always clean up the listener when the object is destroyed
        listenerRegistration?.Stop();
    }
}
