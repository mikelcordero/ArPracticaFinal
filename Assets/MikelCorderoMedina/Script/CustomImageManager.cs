using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CustomImageManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    private bool recorridoLanzado = false;

    void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (recorridoLanzado) return;

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Debug.Log("Imagen detectada, generando recorrido.");
                FindObjectOfType<ArrowPathManager>()?.InitializePath(trackedImage.transform.position);
                recorridoLanzado = true;
            }
        }
    }
}
