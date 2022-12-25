namespace Day17
{
    public class State : IEquatable<State>
    {
        static IEqualityComparer<HashSet<(int, long)>> setComparer = HashSet<(int, long)>.CreateSetComparer();

        public int InputIdx { get; }

        public HashSet<(int, long)> Column { get; }

        public State(int inputPtr, HashSet<(int, long)> column)
        {
            InputIdx = inputPtr;
            Column = column;
        }

        public bool Equals(State? other) =>
            other != null && InputIdx == other.InputIdx && setComparer.Equals(Column, other.Column);

        public override bool Equals(object? obj) =>
            Equals(obj as State);

        public override int GetHashCode() =>
            HashCode.Combine(InputIdx, setComparer.GetHashCode(Column));
    }
}
