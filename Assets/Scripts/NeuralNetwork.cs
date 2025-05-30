// NeuralNetwork.cs - Modificado para arquitetura fixa: 18 entradas + bias → 6 ocultos + bias → 4 saídas
using System;
using UnityEngine;

public class NeuralNetwork
{
    public Layer hiddenLayer;
    public Layer outputLayer;

    public NeuralNetwork()
    {
        int inputCount = 18;
        int hiddenCount = 6;
        int outputCount = 4;

        int inputWithBias = inputCount + 1;
        int hiddenWithBias = hiddenCount + 1;

        hiddenLayer = new Layer(hiddenCount, inputWithBias, useDouble: true);
        outputLayer = new Layer(outputCount, hiddenWithBias, useDouble: true);
    }

    public double[] FeedForward(double[] inputs)
    {
        if (inputs.Length != 18)
        {
            Debug.LogError("A rede espera exatamente 18 inputs (sem contar o bias). Recebido: " + inputs.Length);
            return new double[4];
        }

        double[] extendedInput = new double[19];
        inputs.CopyTo(extendedInput, 0);
        extendedInput[18] = 1.0; // Bias

        double[] hiddenOutput = hiddenLayer.CalculateOutputs(extendedInput);

        double[] extendedHidden = new double[7];
        hiddenOutput.CopyTo(extendedHidden, 0);
        extendedHidden[6] = 1.0; // Bias

        return outputLayer.CalculateOutputs(extendedHidden, false);
    }

    public int TotalWeights()
    {
        return hiddenLayer.TotalWeights() + outputLayer.TotalWeights();
    }

    public void LoadFromDNA(double[] dna)
    {
        int index = 0;
        index = hiddenLayer.SetWeightsFrom(dna, index);
        outputLayer.SetWeightsFrom(dna, index);
    }

    public double[] GetWeightsAsDNA()
    {
        double[] dna = new double[TotalWeights()];
        int index = 0;
        index = hiddenLayer.WriteWeightsTo(dna, index);
        outputLayer.WriteWeightsTo(dna, index);
        return dna;
    }
} 


