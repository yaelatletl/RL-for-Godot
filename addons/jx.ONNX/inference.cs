using Godot;
using System;
using Microsoft.ML.OnnxRuntime;
using System.Numerics.Tensors;
using System.Collections.Generic; // for List

public class inference : Node
{
    private InferenceSession local_session;
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        System.Console.WriteLine("Test Compilation");
    }
    public void LoadModel(string model_path)
    {
        local_session = new InferenceSession(model_path);
    }
    public void RunModel()
    {
        var inputTensor = new DenseTensor<string>(new string[] { review }, new int[] { 1, 1 });
        var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor<string>("input", inputTensor) };
        var results = local_session.Run(inputs);
        foreach (var r in results)
        {
            System.Console.WriteLine(r.AsTensor<float>()[0]);
        }
    }
}

