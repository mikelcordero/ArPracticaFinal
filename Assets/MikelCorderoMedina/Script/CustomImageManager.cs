using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems; 

public class CustomImageManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    

    void Awake()
    {

        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        if (trackedImageManager == null)
        {
            Debug.LogError("LogTrackedImages: ARTrackedImageManager no encontrado en la escena.");
            enabled = false; // Deshabilita este script si no se encuentra el manager.
        }
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }


    

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        Debug.LogWarning($"EVENTO TrackedImagesChanged DISPARADO! Imágenes añadidas: {eventArgs.added.Count}, Actualizadas: {eventArgs.updated.Count}, Eliminadas: {eventArgs.removed.Count}");

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            Debug.Log($"IMAGEN AÑADIDA (Evento): Nombre='{trackedImage.referenceImage.name}', Estado={trackedImage.trackingState}, Posición={trackedImage.transform.position}");
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            Debug.Log($"IMAGEN ACTUALIZADA (Evento): Nombre='{trackedImage.referenceImage.name}', Estado={trackedImage.trackingState}, Posición={trackedImage.transform.position}");
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            Debug.Log($"IMAGEN ELIMINADA (Evento): Nombre='{trackedImage.referenceImage.name}'");
        }
    }
}