using Godot;
using System;
using System.Collections.Generic; // for List
using System.Linq;
//using Microsoft.ML.OnnxRuntime; // dotnet add package Microsoft.ML.OnnxRuntime  // not needed for this model
using Microsoft.ML.OnnxRuntime.Tensors; // dotnet add package Microsoft.ML.OnnxRuntime.Tensors
//using System.Numerics.Tensors; // dotnet add package System.Numerics.Tensors 

//This is model specific, used for pre and post processing
using SixLabors.ImageSharp; // dotnet add package SixLabors.ImageSharp
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace Microsoft.ML.OnnxRuntime.ResNet50v2{
public class inference : Node
{
    private InferenceSession local_session;
    [Export(PropertyHint.File, "*.onnx")]
    public string modelFilePath = "res://addons/jx.ONNX/models/resnet50-v2-7.onnx"; 
    [Export]
    public Godot.StreamTexture godotImage; // just a test image
    
    public override void _Ready()
    {
        GD.Print("Test Compilation");
        resnet50();
    }
    public List<String> resnet50()
    {
        var godotContentImage = godotImage.GetData(); //Godot.Image
        //Now, we convert from Texture to Image and from Image to raw data, then to Rgb24
        SixLabors.ImageSharp.Image<Rgb24> image = 
        //SixLabors.ImageSharp.Image.Load<Rgb24>(godotImage.GetData().GetData(), out IImageFormat format); // Odd overload, but it works
        SixLabors.ImageSharp.Image.LoadPixelData<Rgb24>(godotContentImage.GetData(), godotContentImage.GetWidth(), godotContentImage.GetHeight()); //Missing: out IImageFormat format parameter in overloads
        var format = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance; //This is a hack, but it works
        List<String> labels = new List<String>();

       // SixLabors.ImageSharp.Image<Rgb24> image =  SixLabors.ImageSharp.Image.Load<Rgb24>(imageFilePath, out IImageFormat format);
            //dotnet add package SixLabors.ImageSharp 
        System.IO.Stream imageStream = new System.IO.MemoryStream(); 

        image.Mutate(x =>
        {
            x.Resize(new ResizeOptions
            {
                Size = new Size(224, 224),
                Mode = ResizeMode.Crop
            });
        });
        image.Save(imageStream, format); // depends on the image load line above
        Tensor<float> input = new DenseTensor<float>(new[] { 1, 3, 224, 224 });
        var mean = new[] { 0.485f, 0.456f, 0.406f };
        var stddev = new[] { 0.229f, 0.224f, 0.225f };
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgb24> pixelSpan = accessor.GetRowSpan(y);
                for (int x = 0; x < accessor.Width; x++)
                {
                    input[0, 0, y, x] = ((pixelSpan[x].R / 255f) - mean[0]) / stddev[0];
                    input[0, 1, y, x] = ((pixelSpan[x].G / 255f) - mean[1]) / stddev[1];
                    input[0, 2, y, x] = ((pixelSpan[x].B / 255f) - mean[2]) / stddev[2];
                }
        }});
        var inputs = new List<NamedOnnxValue>{    
        NamedOnnxValue.CreateFromTensor<float>("data", input)};
        Godot.File file = new Godot.File();
        file.Open(modelFilePath, Godot.File.ModeFlags.Read);
        byte[] model = file.GetBuffer((int)file.GetLen());
        file.Close();
        var session = new InferenceSession(model); //May need to make a wrapper class for this
        IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
        IEnumerable<float> output = results.First().AsEnumerable<float>();
        float sum = output.Sum(x => (float)Math.Exp(x));
        IEnumerable<float> softmax = output.Select(x => (float)Math.Exp(x) / sum);
        IEnumerable<Prediction> top10 = softmax.Select((x, i) => new Prediction { Label = LabelMap.Labels[i], Confidence = x }).OrderByDescending(x => x.Confidence).Take(10);
        GD.Print("Top 10 predictions for ResNet50 v2...");
        GD.Print("--------------------------------------------------------------");
        foreach (var t in top10){
            GD.Print($"Label: {t.Label}, Confidence: {t.Confidence}");
            labels.Add(t.Label);
        }
        return labels;
    } 

}

}