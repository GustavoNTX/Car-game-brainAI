// Layer.cs - Atualizado para usar double
public class Layer
{

    private Neuron[] neurons;

    public Layer(int neuronCount, int inputCountPerNeuron, bool useDouble = false)
    {
        neurons = new Neuron[neuronCount];
        for (int i = 0; i < neuronCount; i++)
        {
            neurons[i] = new Neuron(inputCountPerNeuron, useDouble);
        }
    }

    public double[] CalculateOutputs(double[] inputs, bool addBias = true)
    {
        double[] outputs = new double[neurons.Length];
        for (int i = 0; i < neurons.Length; i++)
        {
            outputs[i] = neurons[i].Activate(inputs);
        }
        return outputs;
    }

    public int TotalWeights()
    {
        int total = 0;
        foreach (var neuron in neurons)
            total += neuron.weights.Length;
        return total;
    }

    public int SetWeightsFrom(double[] dna, int start)
    {
        int index = start;
        foreach (var neuron in neurons)
        {
            for (int i = 0; i < neuron.weights.Length; i++)
            {
                neuron.weights[i] = dna[index++];
            }
        }
        return index;
    }

    public int WriteWeightsTo(double[] dna, int start)
    {
        int index = start;
        foreach (var neuron in neurons)
        {
            for (int i = 0; i < neuron.weights.Length; i++)
            {
                dna[index++] = neuron.weights[i];
            }
        }
        return index;
    }

}