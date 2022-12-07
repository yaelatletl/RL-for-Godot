using Godot;
using System;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp; // dotnet add package SixLabors.ImageSharp
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

public static class ImageUtils 
{
    // Converts a Godot Image to a Tensor<float>
    public static Tensor<float> ToTensor(Godot.Image image)
    {
        var tensor = new DenseTensor<float>(new[] { 1, 3, image.GetHeight(), image.GetWidth() });
        var data = image.GetData();
        for (int i = 0; i < data.Length; i += 4)
        {
            tensor[i / 4] = data[i + 2] / 255f;
            tensor[i / 4 + 1] = data[i + 1] / 255f;
            tensor[i / 4 + 2] = data[i] / 255f;
        }
        return tensor;
    }

    // Converts a Tensor<float> to a Godot Image
    public static Godot.Image ToImage(Tensor<float> tensor)
    {
        var image = new Godot.Image();
        image.Create(tensor.Dimensions[3], tensor.Dimensions[2], false, Godot.Image.Format.Rgb8);
        var data = image.GetData();
        for (int i = 0; i < data.Length; i += 4)
        {
            data[i] = (byte)(tensor[i / 4 + 2] * 255);
            data[i + 1] = (byte)(tensor[i / 4 + 1] * 255);
            data[i + 2] = (byte)(tensor[i / 4] * 255);
            data[i + 3] = 255;
        }
        return image;
    }

    // Converts a Godot Image to a Labors ImageSharp Image<Rgb24>, add out IImageFormat format parameter in overloads
    public static SixLabors.ImageSharp.Image<Rgb24> ToImageSharp(Godot.Image image, out IImageFormat format)
    {
        //Now, we convert from Texture to Image and from Image to raw data, then to Rgb24
        SixLabors.ImageSharp.Image<Rgb24> sharpImage = SixLabors.ImageSharp.Image.LoadPixelData<Rgb24>(image.GetData(), image.GetWidth(), image.GetHeight()); 
        //Missing: out IImageFormat format parameter in overloads
        format = SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance; //This is a hack, but it works
        return sharpImage;
    }
    
    // TODO: Converts a Labors ImageSharp Image<Rgb24> to a Godot Image

}
