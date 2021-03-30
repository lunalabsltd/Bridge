using System.Collections.Generic;
using Bridge;

namespace System.Collections
{
    internal static class HashHelpers
    {
        private static byte[] data = new byte[1024];
        private static int currentIndex = 1024;
        private static readonly object lockObj = new object();

        private const Int32 HashPrime = 101;

        public static readonly int RandomSeed = Guid.NewGuid().GetHashCode();

        public static int Combine(int h1, int h2)
        {
            unchecked
            {
                uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
                return ((int)rol5 + h1) ^ h2;
            }
        }

        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369 };

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        public static int GetPrime(int min)
        {
            if (min < 0)
                throw new ArgumentException("Hashtable's capacity overflowed and went negative. Check load factor, capacity and the current size of the table.");
            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min)
                    return prime;
            }
            for (int i = (min | 1); i < Int32.MaxValue; i += 2)
            {
                if (IsPrime(i) && ((i - 1) % HashPrime != 0))
                    return i;
            }
            return min;
        }

        public static int GetMinPrime()
        {
            return primes[0];
        }

        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;
            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            {
                return MaxPrimeArrayLength;
            }
            return GetPrime(newSize);
        }

        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        public static bool IsWellKnownEqualityComparer(object comparer)
        {
            if (comparer != null && comparer != EqualityComparer<string>.Default)
                return comparer is IWellKnownStringEqualityComparer;
            return true;
        }

        public static IEqualityComparer GetRandomizedEqualityComparer(object comparer)
        {
            if (comparer == null)
                return (IEqualityComparer) new RandomizedObjectEqualityComparer();
            if (comparer == EqualityComparer<string>.Default)
                return (IEqualityComparer) new RandomizedStringEqualityComparer();
            return (comparer as IWellKnownStringEqualityComparer)?.GetRandomizedEqualityComparer();
        }

        internal static long GetEntropy()
        {
            lock (HashHelpers.lockObj)
            {
                if (HashHelpers.currentIndex == 1024)
                {
                    GetCryptoBytes(HashHelpers.data);
                    HashHelpers.currentIndex = 0;
                }
                long int64 = BitConverter.ToInt64(HashHelpers.data, HashHelpers.currentIndex);
                HashHelpers.currentIndex += 8;
                return int64;
            }
        }

        public static void GetCryptoBytes( byte[] array ) {
            Script.Write( @" var bytesArray = new Uint8Array(array.length);
                         window.crypto.getRandomValues(bytesArray);
                         for(var i = 0; i < array.length; i++) {
                             array[i] = bytesArray[i];
                          }" );
        }
    }
}
