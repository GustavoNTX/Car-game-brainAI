// NavigationMapGenerator.cs - Ajustado para escala de 100 pixels por unidade e worldSize correto
using UnityEngine;
using System.Collections.Generic;

public class NavigationMapGenerator : MonoBehaviour
{
    public static NavigationMapGenerator Instance;

    public Texture2D pistaImage;
    public Vector2 worldSize = new Vector2(13.5f, 13.5f); // mantenha conforme real na Unity
    public Vector2 goalWorldPosition;
    public int resolution = 128; // número de células por eixo

    public int[,] binaryMap; // 0 = parede, 1 = pista
    public int[,] waveMap;   // distância real até o objetivo
    public int maxWaveValue = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateBinaryMap();
        GenerateWaveMap();
    }

    void GenerateBinaryMap()
    {
        binaryMap = new int[resolution, resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                // converter para coordenadas da imagem
                float u = x / (float)(resolution - 1);
                float v = y / (float)(resolution - 1);

                Color pixel = pistaImage.GetPixelBilinear(u, v);

                if (pixel.r > 0.2f && pixel.g > 0.2f && pixel.b > 0.2f) // cinza claro da pista
                    binaryMap[x, y] = 1; // pista
                else
                    binaryMap[x, y] = 0; // parede
            }
        }
    }

    void GenerateWaveMap()
    {
        waveMap = new int[resolution, resolution];

        for (int y = 0; y < resolution; y++)
            for (int x = 0; x < resolution; x++)
                waveMap[x, y] = -1; // não visitado

        Vector2Int goalCell = WorldToGrid(goalWorldPosition);

        if (binaryMap[goalCell.x, goalCell.y] == 0)
        {
            Debug.LogError("O ponto de chegada está em uma parede!");
            return;
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        waveMap[goalCell.x, goalCell.y] = 0;
        queue.Enqueue(goalCell);

        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int cost = waveMap[current.x, current.y];
            maxWaveValue = Mathf.Max(maxWaveValue, cost);

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (InBounds(neighbor) && binaryMap[neighbor.x, neighbor.y] == 1 && waveMap[neighbor.x, neighbor.y] == -1)
                {
                    waveMap[neighbor.x, neighbor.y] = cost + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        Debug.Log("Wavefront gerado com sucesso. Max Value: " + maxWaveValue);
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        // converte coordenada do mundo para célula no grid
        float normalizedX = (worldPos.x + worldSize.x / 2f) / worldSize.x;
        float normalizedY = (worldPos.y + worldSize.y / 2f) / worldSize.y;

        int gridX = Mathf.Clamp((int)(normalizedX * resolution), 0, resolution - 1);
        int gridY = Mathf.Clamp((int)(normalizedY * resolution), 0, resolution - 1);

        return new Vector2Int(gridX, gridY);
    }

    public int GetWaveValue(Vector2 worldPos)
    {
        Vector2Int cell = WorldToGrid(worldPos);
        return waveMap[cell.x, cell.y];
    }

    private bool InBounds(Vector2Int cell)
    {
        return cell.x >= 0 && cell.y >= 0 && cell.x < resolution && cell.y < resolution;
    }

    public List<Vector2> GetPathFrom(Vector2 start)
{
    List<Vector2> path = new List<Vector2>();
    Vector2Int current = WorldToGrid(start);

    if (!InBounds(current) || waveMap[current.x, current.y] == -1)
        return path;

    path.Add(GridToWorld(current));

    while (waveMap[current.x, current.y] > 0)
    {
        Vector2Int next = current;
        int bestValue = waveMap[current.x, current.y];

        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (var dir in directions)
        {
            Vector2Int neighbor = current + dir;
            if (InBounds(neighbor) && waveMap[neighbor.x, neighbor.y] >= 0 &&
                waveMap[neighbor.x, neighbor.y] < bestValue)
            {
                bestValue = waveMap[neighbor.x, neighbor.y];
                next = neighbor;
            }
        }

        if (next == current) break;

        current = next;
        path.Add(GridToWorld(current));
    }

    return path;
}

public Vector2 GridToWorld(Vector2Int gridPos)
{
    float x = ((float)gridPos.x / resolution) * worldSize.x - worldSize.x / 2f;
    float y = ((float)gridPos.y / resolution) * worldSize.y - worldSize.y / 2f;
    return new Vector2(x, y);
}



}
