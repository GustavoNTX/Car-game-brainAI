using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject laserPrefab;
    public Transform spawnPoint;
    public int populationSize = 1000;
    public float generationTime = 10f;
    public float spawnDelay = 0.05f;

    private List<GameObject> cars = new List<GameObject>();
    private List<double[]> populationDNA = new List<double[]>();
    private float timer = 0f;
    private int generation = 0;

    void Start()
    {
        StartCoroutine(SpawnGeneration());
    }

    void Update()
    {
        timer += Time.deltaTime;

        bool allDead = true;
        foreach (var car in cars)
        {
            if (car.activeSelf)
            {
                allDead = false;
                break;
            }
        }

        if (allDead || timer >= generationTime)
        {
            timer = 0f;
            Evolve();
        }
    }

    IEnumerator SpawnGeneration()
    {
        foreach (GameObject car in cars)
            Destroy(car);

        cars.Clear();

        if (populationDNA.Count == 0)
        {
            for (int i = 0; i < populationSize; i++)
            {
                var net = new NeuralNetwork();
                populationDNA.Add(net.GetWeightsAsDNA());
            }
        }

        for (int i = 0; i < populationSize; i++)
        {
            GameObject car = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
            CarController controller = car.GetComponent<CarController>();
            controller.brain = new NeuralNetwork();
            controller.brain.LoadFromDNA(populationDNA[i]);
            cars.Add(car);

            yield return new WaitForSeconds(spawnDelay);
        }

        if (laserPrefab != null)
        {
            var laserObj = Instantiate(laserPrefab);
            var laser = laserObj.GetComponent<LaserFollower>();
        
        }


        generation++;
        Debug.Log($"Geração: {generation}");
    }

    void Evolve()
    {
        List<(double[], float)> evaluated = new List<(double[], float)>();

        foreach (GameObject car in cars)
        {
            var c = car.GetComponent<CarController>();
            evaluated.Add((c.brain.GetWeightsAsDNA(), (float)c.fitness));
        }

        evaluated.Sort((a, b) => b.Item2.CompareTo(a.Item2));

        float bestFitness = evaluated[0].Item2;
        Debug.Log($"Melhor fitness da geração {generation}: {bestFitness:F2}");

        populationDNA.Clear();

        int eliteCount = Mathf.Max(1, populationSize / 20);
        for (int i = 0; i < eliteCount; i++)
            populationDNA.Add(evaluated[i].Item1);

        while (populationDNA.Count < populationSize)
        {
            var parent = evaluated[Random.Range(0, populationSize / 2)].Item1;
            populationDNA.Add(Mutate(parent));
        }

        StartCoroutine(SpawnGeneration());
    }

    double[] Mutate(double[] dna)
    {
        double[] newDNA = new double[dna.Length];
        for (int i = 0; i < dna.Length; i++)
        {
            int type = Random.Range(0, 3);
            switch (type)
            {
                case 0:
                    newDNA[i] = Random.Range(-1f, 1f); break;
                case 1:
                    double factor = Random.Range(0.5f, 1.5f);
                    newDNA[i] = dna[i] * factor; break;
                case 2:
                    newDNA[i] = dna[i] + Random.Range(-0.1f, 0.1f); break;
            }
        }
        return newDNA;
    }
    
    public bool AllCarsDead()
{
    foreach (var car in cars)
    {
        if (car.activeSelf) return false;
    }
    return true;
}

}
