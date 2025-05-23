using System.Collections.Generic;
using UnityEngine;

public class ArrowPathManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float activationDistance = 1.2f;
    public float spacing = 2.5f;

    private List<Vector3> pathPoints = new List<Vector3>();
    private int currentIndex = 0;
    private GameObject currentArrow;
    private bool pathInitialized = false;

    public void InitializePath(Vector3 imagePosition)
    {
        if (pathInitialized) return;
        pathInitialized = true;

        float groundY = 0.01f;
        Vector3 startPos = new Vector3(imagePosition.x, groundY, imagePosition.z);
        Vector3 current = startPos;

        // 🔁 Camino lógico: avanzar 3 veces, luego girar a la izquierda 3 veces
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,  // paso 1
            Vector3.forward,  // paso 2
            Vector3.forward,  // paso 3
            Vector3.left,     // paso 4
            Vector3.left,     // paso 5
            Vector3.left      // paso 6
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

        float distance = Vector3.Distance(Camera.main.transform.position, currentArrow.transform.position);
        if (distance < activationDistance)
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

        Vector3 from = currentIndex == 0
            ? pathPoints[0] - Vector3.forward * spacing // punto anterior ficticio
            : pathPoints[currentIndex - 1];

        Vector3 to = pathPoints[currentIndex];
        Vector3 dir = (to - from).normalized;

        // 🔁 Instanciar la flecha mirando a la dirección + corrección de -90º para que apunte visualmente bien
        currentArrow = Instantiate(arrowPrefab, to, Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0));
    }
}
