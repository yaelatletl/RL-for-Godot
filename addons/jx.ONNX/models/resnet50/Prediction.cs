namespace Microsoft.ML.OnnxRuntime.ResNet50v2
{
    internal class Prediction
    {
        public string Label { get; set; }
        public float Confidence { get; set; }
    }
}