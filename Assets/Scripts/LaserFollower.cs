using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserFollower : MonoBehaviour
{
    public float speed = 2f;

    private List<Vector2> path;
    private int currentIndex = 0;
    private LineRenderer line;
    private CarSpawner spawner; // Referência para o CarSpawner

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;


        // Pegamos o ponto inicial do spawnPoint ao invés do goal
        Vector2 start = spawner.spawnPoint.position;
        path = NavigationMapGenerator.Instance.GetPathFrom(start); // ← Certifique-se de ter esse método criado

        if (path == null || path.Count == 0)
        {
            Debug.LogError("Caminho do laser não encontrado.");
            Destroy(gameObject);
            return;
        }

        transform.position = path[0];
        DrawPathLine();
    }

    void Update()
    {
        if (spawner == null || spawner.AllCarsDead()) // Checa se todos os carros morreram
        {
            Destroy(gameObject);
            return;
        }

        if (currentIndex >= path.Count) return;

        Vector2 target = path[currentIndex];
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            currentIndex++;
    }

    void DrawPathLine()
    {
        line.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            line.SetPosition(i, new Vector3(path[i].x, path[i].y, 0));
        }
    }
}
