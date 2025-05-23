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
        if (trackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager no encontrado.");
            enabled = false;
        }
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (recorridoLanzado) return;

        // Solo procesamos imágenes que han sido actualizadas
        foreach (var img in args.updated)
        {
            // Si ha pasado a Tracking y además está dentro del viewport de la cámara...
            if (img.trackingState == TrackingState.Tracking &&
                img.transform.localScale.x > 0.01f &&
                IsVisibleToCamera(img.transform.position))
            {
                Debug.Log("🔍 Imagen en Tracking y visible → iniciando recorrido");
                FindObjectOfType<ArrowPathManager>()?.InitializePath(img.transform.position);
                recorridoLanzado = true;
                break;
            }
        }
    }

    // Comprueba si worldPos está dentro de Camera.main.view
    bool IsVisibleToCamera(Vector3 worldPos)
    {
        var cam = Camera.main;
        if (cam == null) return false;
        Vector3 vp = cam.WorldToViewportPoint(worldPos);
        return vp.z > 0 && vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
    }
}
