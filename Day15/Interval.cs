namespace Day15
{
    public class Interval : IComparable<Interval>
    {
        public int Lo { get; set; }
        public int Hi { get; set; }

        public Interval(int x, int y)
        {
            Lo = x;
            Hi = y;
        }

        // override Equals and GetHashCode so we can use a HashSet
        public override bool Equals(Object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Interval other = (Interval)obj;
                return (Lo == other.Lo && Hi == other.Hi);
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Lo, Hi).GetHashCode();
        }

        // override ToString for easy printing
        public override string ToString()
        {
            return string.Format($"[{Lo},{Hi}]");
        }

        public int CompareTo(Interval other) => (Lo, Hi).CompareTo((other.Lo, other.Hi));

        //public int CompareTo(Interval? other)
        //{
        //    if (Lo < other.Lo)
        //        return -1;
        //    if (Lo > other.Lo)
        //        return 1;
        //    if (Lo == other.Lo)
        //    {
        //        if (Hi < other.Hi)
        //            return -1;
        //        if (Hi > other.Hi)
        //            return 1;
        //        return 0;
        //    }
        //    return 0;
        //}
    }
}
