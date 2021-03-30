using System.Collections;

namespace System
{
    public interface IWellKnownStringEqualityComparer
    {
        IEqualityComparer GetRandomizedEqualityComparer();

        IEqualityComparer GetEqualityComparerForSerialization();
    }
}