namespace TextRecognizer
{
    public class Neuron
    {
        public string name;
        public float[,] matches;
        public float[,] weight;
        public float[,] input;
        public float sumOfMatches;
        public float limit = 180;

        public Neuron(string name)
        {
            this.name = name;
        }
        public void SetResolution(int width, int height)
        {
            matches = new float[height, width];
            weight = new float[height, width];
            input = new float[height, width];
        }
        public static bool IsWhite(float pixel)
        {
            return pixel > MyColors.NearWhite;
        }
    }
}

