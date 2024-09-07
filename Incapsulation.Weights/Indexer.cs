namespace Incapsulation.Weights;

public class Indexer
{
    public int Start
    {
        get => start;
        private set
        {
            if (value < 0 || value > array.Length || value + length > array.Length)
            {
                throw new ArgumentException();
            }

            start = value;
        }
    }

    public int Length
    {
        get => length;
        private set
        {
            if ((array.Length < value + Start) || value < 0)
                throw new ArgumentException();
            length = value;
        }
    }
    
    private readonly double[] array;
    private int length;
    private int start;

    public Indexer(double[] array, int start, int length)
    {
        this.array = array;
        Length = length;
        Start = start;
    }

    public double this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException();
            return array[start + index];
        }
        set
        {
            if (index < 0 || index >= Length)
                throw new IndexOutOfRangeException();
            array[start + index] = value;
        }
    }
}