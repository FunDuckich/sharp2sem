using System;

namespace sharp2sem._22_3
{
    public class City
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int OriginalIndex { get; set; }

        public City(string name, int x, int y, int originalIndex)
        {
            Name = name;
            X = x;
            Y = y;
            OriginalIndex = originalIndex;
        }

        public double DistanceTo(City otherCity)
        {
            return Math.Sqrt(Math.Pow(X - otherCity.X, 2) + Math.Pow(Y - otherCity.Y, 2));
        }
    }
}