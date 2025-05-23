using System.Collections.Generic;
using UnityEngine;

public class ArrowPathManager : MonoBehaviour
{
    [Tooltip("Prefab de la flecha")]
    public GameObject arrowPrefab;
    [Tooltip("Distancia a la flecha para activar la siguiente")]
    public float activationDistance = 1.2f;
    [Tooltip("Separación en metros entre flechas")]
    public float spacing = 2.5f;

    private List<Vector3> pathPoints = new List<Vector3>();
    private int currentIndex = 0;
    private GameObject currentArrow;
    private bool pathInitialized = false;

    /// <summary>
    /// Llamar una vez, cuando la imagen pase a Tracking y sea visible.
    /// </summary>
    public void InitializePath(Vector3 imagePosition)
    {
        if (pathInitialized) return;
        pathInitialized = true;

        float groundY = 0.01f;
        Vector3 current = new Vector3(imagePosition.x, groundY, imagePosition.z);

        // Avanzar 3 pasos, luego girar a la izquierda 3 pasos
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

        // Apunta en dir + rotación extra -90° para corregir el prefab
        currentArrow = Instantiate(
            arrowPrefab,
            to,
            Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0)
        );
    }
}
