# Master Thesis: CloudXR AR Streaming to Apple Vision Pro

This Unity project serves as the server-side application for streaming interactive Augmented Reality (AR) experiences to an Apple Vision Pro using NVIDIA CloudXR. 

The project is configured to run as a Windows executable that streams a transparent pass-through background alongside interactable 3D elements, allowing the Vision Pro user to seamlessly see the virtual objects overlaid in their physical environment.

## Key Features

- **NVIDIA CloudXR Integration**: Streams high-fidelity graphics directly to the Apple Vision Pro client.
- **Transparent AR Pass-through**: Configured to stream with an alpha channel to enable true augmented reality (rather than fully immersive VR) on the Vision Pro.
- **OpenXR Interaction Profiles**: Maps Vision Pro inputs (such as pinch gestures and spatial pointer tracking) to standard OpenXR bindings.
- **Interactive Objects**: Includes interactive systems such as object grabbing (`MoveCube`), and material state management (`FadeAlpha0`, `FadeAlpha1`) triggered via pointer interactions.

## Project Structure

- `Assets/MRTemplateAssets/`: Contains the core assets, models, materials, and scripts for the AR template.
- `Assets/VisionOS_InputActions.inputactions`: The custom Unity Input System asset mapping the Vision Pro's hand tracking and pinch gestures.
  - **Pinch Action**: Mapped to `<XRController>{RightHand}/select`
  - **Pointer Pose Action**: Mapped to `<XRController>{RightHand}/pointer`
- `Assets/MRTemplateAssets/Scripts/`: 
  - `VerifyVisionOSInput.cs`: A utility script using TextMeshPro to visually debug and verify that the Vision Pro's spatial pointer and pinch actions are being correctly received by the CloudXR server.
  - `MoveCube.cs`: Handles grab/move interactions.
  - `FadeAlpha0.cs` & `FadeAlpha1.cs`: Handles interactive material fading and temporary color changes upon interaction.

## Prerequisites & Setup

1. **Unity Version**: Ensure you are using the correct Unity version compatible with OpenXR and the XR Interaction Toolkit.
2. **CloudXR Server**: The host Windows machine must have the NVIDIA CloudXR server components installed and running.
3. **Packages**: The project relies on:
   - `com.unity.xr.openxr`
   - `com.unity.inputsystem`
   - `com.unity.xr.interaction.toolkit` (For advanced interactions)
4. **Build Settings**: The application must be built as a Windows Standalone executable to act as the CloudXR server.

## Running the Server

To launch the project for the Apple Vision Pro:
1. Build the Unity project to your desired directory (e.g., `ARBuild`).
2. Run the custom `Start_CloudXR.bat` script. This script configures the necessary environment variables and points the CloudXR server to the newly built `.exe`.
3. Launch the CloudXR client application on the Apple Vision Pro and connect to the server's IP address.

## Troubleshooting Inputs

If inputs from the Vision Pro are not registering:
1. Add the `VerifyVisionOSInput.cs` script to an empty GameObject in the scene.
2. Assign a TextMeshPro UI text component to it.
3. Assign the `Pinch Action` and `Pointer Pose Action` from `VisionOS_InputActions.inputactions` using the inspector reference slots.
4. Put on the headset and verify if the data stream changes the UI text when pinching and moving your hands.
