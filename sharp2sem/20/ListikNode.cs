namespace sharp2sem
{
    public class ListikNode
    {
        public int Value { get; set; }
        public ListikNode Next { get; set; }

        public ListikNode(int value)
        {
            Value = value;
            Next = null;
        }
    }
}
