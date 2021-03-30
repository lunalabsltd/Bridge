namespace System.Collections.Generic
{
    public class RandomizedStringEqualityComparer : IEqualityComparer<string>, IEqualityComparer, IWellKnownStringEqualityComparer
    {
        private long _entropy;

        public RandomizedStringEqualityComparer()
        {
            this._entropy = HashHelpers.GetEntropy();
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x == y)
                return true;
            if (x == null || y == null)
                return false;
            if (x is string && y is string)
                return this.Equals((string) x, (string) y);
            ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArgumentForComparison);
            return false;
        }

        public bool Equals(string x, string y)
        {
            if (x != null)
            {
                if (y != null)
                    return x.Equals(y);
                return false;
            }
            return y == null;
        }

        public int GetHashCode(string obj)
        {
            if (obj == null)
                return 0;
            return obj.GetHashCode();
        }

        public int GetHashCode(object obj)
        {
            if (obj == null)
                return 0;
            return obj.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            RandomizedStringEqualityComparer equalityComparer = obj as RandomizedStringEqualityComparer;
            if (equalityComparer != null)
                return this._entropy == equalityComparer._entropy;
            return false;
        }

        public override int GetHashCode()
        {
            return this.GetType().Name.GetHashCode() ^ (int) (this._entropy & (long) int.MaxValue);
        }

        IEqualityComparer IWellKnownStringEqualityComparer.GetRandomizedEqualityComparer()
        {
            return (IEqualityComparer) new RandomizedStringEqualityComparer();
        }

        IEqualityComparer IWellKnownStringEqualityComparer.GetEqualityComparerForSerialization()
        {
            return (IEqualityComparer) EqualityComparer<string>.Default;
        }
    }
}