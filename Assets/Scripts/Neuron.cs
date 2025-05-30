using UnityEngine;

public class Neuron
{
    public double[] weights;

    public Neuron(int inputCount, bool useDouble = false)
{
    weights = new double[inputCount];
    for (int i = 0; i < inputCount; i++)
    {
        weights[i] = UnityEngine.Random.Range(-1f, 1f);
    }
}


    public double Activate(double[] inputs)
    {
        double sum = 0.0;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += inputs[i] * weights[i];
        }
        return ReLU(sum);
    }

    private double ReLU(double x)
    {
        return x < 0 ? 0 : (x > 10000 ? 10000 : x);
    }
}
