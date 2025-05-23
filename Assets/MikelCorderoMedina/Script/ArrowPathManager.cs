using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ArrowPathManager : MonoBehaviour
{
    [Header("Flechas")]
    [Tooltip("Prefab de la flecha")]
    public GameObject arrowPrefab;
    [Tooltip("Separación en metros entre flechas")]
    public float spacing = 2.5f;
    [Tooltip("Distancia a la flecha para activarla (y reproducir el sonido)")]
    public float activationDistance = 1.2f;

    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al pasar cada flecha")]
    public AudioClip passSound;

    private AudioSource audioSource;
    private List<Vector3> pathPoints = new List<Vector3>();
    private int currentIndex = 0;
    private GameObject currentArrow;
    private bool pathInitialized = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// Llamar una sola vez cuando la imagen pase a Tracking y sea visible.
    public void InitializePath(Vector3 imagePosition)
    {
        if (pathInitialized) return;
        pathInitialized = true;

        float groundY = 0.01f;
        Vector3 current = new Vector3(imagePosition.x, groundY, imagePosition.z);

        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            Vector3.left,
            Vector3.left,
            Vector3.left
        };

        foreach (Vector3 dir in directions)
        {
            current += dir * spacing;
            pathPoints.Add(current);
        }

        CreateNextArrow();
    }

    void Update()
    {
        if (!pathInitialized || currentArrow == null) return;

        float dist = Vector3.Distance(Camera.main.transform.position, currentArrow.transform.position);
        if (dist < activationDistance)
        {
            if (passSound != null)
                audioSource.PlayOneShot(passSound);

            Destroy(currentArrow);
            currentArrow = null;
            currentIndex++;

            CreateNextArrow();
        }
    }

    void CreateNextArrow()
    {
        if (currentIndex >= pathPoints.Count) return;

        Vector3 from = (currentIndex == 0)
            ? pathPoints[0] - Vector3.forward * spacing
            : pathPoints[currentIndex - 1];

        Vector3 to = pathPoints[currentIndex];
        Vector3 dir = (to - from).normalized;

        // Rotación corregida -90° para que tu prefab apunte bien
        currentArrow = Instantiate(
            arrowPrefab,
            to,
            Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0)
        );
    }
}
