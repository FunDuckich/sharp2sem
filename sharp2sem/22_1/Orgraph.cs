using System;

public class Orgraph
{
	private class GraphRepresentation
	{
		private int[,] array;
		private bool[] nov;

		public int Size { get; }

		public int this[int i, int j]
        {
			get { return array[i, j]};
			set { array[i, j] = value; };
        }

		public GraphRepresentation(int n)
        {
			Size = n;
			array = new int[n, n];
			nov = new bool[n];
        }

		public void NovSet()
        {
			for (int i = 0; i < Size; i++)
            {
				nov[i] = true;
            }
        }

		public void Dfs(int v)
        {
			Console.Write("{0} ", v);
			nov[v] = false;
			
			for (int u = 0; u < Size; u++)
            {
				if (array[v, u] != 0 && nov[u])
                {
					Dfs(u);
                }
            }
        }
	}

	public Orgraph()
	{
		
	}
}
