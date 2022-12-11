namespace Day09
{
    public class Knot
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Knot(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(string direction)
        {
            if (direction == "R")
                X += 1;

            if (direction == "L")
                X -= 1;

            if (direction == "U")
                Y += 1;

            if (direction == "D")
                Y -= 1;
        }

        // override Equals and GetHashCode so we can use a HashSet
        public override bool Equals(Object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Knot other = (Knot)obj;
                return (X == other.X && Y == other.Y);
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"[{X},{Y}]");
        }
    }
}
