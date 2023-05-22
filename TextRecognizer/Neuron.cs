namespace TextRecognizer
{
    public class Neuron
    {
        public string name;
        public float[,] input;
        public float[,] weights;
        public float[,] matches;
        public float sumOfMatches;
        public float limit = 32000;

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
    }
}

