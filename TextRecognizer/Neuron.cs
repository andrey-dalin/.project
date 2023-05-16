namespace TextRecognizer
{
    public class Neuron
    {
        public string name;
        public float[,] input;
        public float[,] weights;
        public float[,] matches;
        public float sumOfMatches;
        public float limit = 180;

        public Neuron(string name)
        {
            this.name = name;
        }
        public void SetResolution(int width, int height)
        {
            matches = new float[height, width];
            weights = new float[height, width];
            input = new float[height, width];
        }
        public static bool IsWhite(float pixel)
        {
            return pixel > MyColors.NearWhite;
        }
    }
}

