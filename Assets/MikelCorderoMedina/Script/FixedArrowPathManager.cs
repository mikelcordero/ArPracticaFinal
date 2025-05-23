using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FixedArrowPathManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float activationDistance = 1f;

    private List<GameObject> arrows = new List<GameObject>();
    private int currentIndex = 0;
    private bool pathInitialized = false;

    // Posiciones fijas del recorrido en el escenario XR (ajusta a tu escena)
    private Vector3[] fixedWorldPositions = new Vector3[]
    {
        new Vector3(0f, 0.01f, 0f),
        new Vector3(1.5f, 0.01f, 0f),
        new Vector3(3f, 0.01f, 0f),
        new Vector3(4.5f, 0.01f, 0f)
    };

    void Update()
    {
        if (!pathInitialized || currentIndex >= arrows.Count)
            return;

        GameObject currentArrow = arrows[currentIndex];
        float distance = Vector3.Distance(Camera.main.transform.position, currentArrow.transform.position);

        if (distance < activationDistance)
        {
            Destroy(currentArrow);
            currentIndex++;

            if (currentIndex < fixedWorldPositions.Length)
                CreateArrow(currentIndex);
        }
    }

    public void InitializePath()
    {
        if (pathInitialized) return;

        pathInitialized = true;
        CreateArrow(0);
    }

    void CreateArrow(int index)
    {
        GameObject arrow = Instantiate(arrowPrefab, fixedWorldPositions[index], Quaternion.identity);
        arrows.Add(arrow);
    }
}
