namespace sharp2sem._20
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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
