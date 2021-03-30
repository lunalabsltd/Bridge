using System;
using Bridge;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace System.Collections {

  /// <summary>
  /// Hashtable.
  /// </summary>
#pragma warning disable 649
#pragma warning disable 169
  public class Hashtable : IDictionary, ICollection, IEnumerable, ISerializable, /*IDeserializationCallback,*/ ICloneable {
    internal const int HashPrime = 101;
    private const int InitialSize = 3;
    private const string LoadFactorName = "LoadFactor";
    private const string VersionName = "Version";
    private const string ComparerName = "Comparer";
    private const string HashCodeProviderName = "HashCodeProvider";
    private const string HashSizeName = "HashSize";
    private const string KeysName = "Keys";
    private const string ValuesName = "Values";
    private const string KeyComparerName = "KeyComparer";
    private Hashtable.bucket[] buckets;
    private int count;
    private int occupancy;
    private int loadsize;
    private float loadFactor;
    private volatile int version;
    private volatile bool isWriterInProgress;
    private ICollection keys;
    private ICollection values;
    private IEqualityComparer _keycomparer;
    private object _syncRoot;

    #region Compatibility fixes

    [ Init( InitPosition.After ) ]
    public static void FixGetMethod() {
      Script.Write(
        " System.Collections.Hashtable.prototype.get = function (key) { var entry = this.findEntry(key); return entry ? entry.value : null; } " );
    }

    #endregion

    protected IEqualityComparer EqualityComparer => this._keycomparer;

    internal Hashtable( bool trash ) { }

    public Hashtable() : this( 0, 1f ) { }

    public Hashtable( int capacity ) : this( capacity, 1f ) { }

    public Hashtable( int capacity, float loadFactor ) {
      if ( capacity < 0 ) {
        throw new ArgumentOutOfRangeException( nameof( capacity ), "ArgumentOutOfRange_NeedNonNegNum" );
      }

      if ( loadFactor < 0.100000001490116 || loadFactor > 1.0 ) {
        throw new ArgumentOutOfRangeException( nameof( loadFactor ), "ArgumentOutOfRange_HashtableLoadFactor" );
      }

      this.loadFactor = 0.72f * loadFactor;
      double num = capacity / this.loadFactor;
      if ( num > int.MaxValue ) {
        throw new ArgumentException( "Arg_HTCapacityOverflow" );
      }

      int length = num > 3.0 ? HashHelpers.GetPrime( ( int ) num ) : 3;
      this.buckets = new Hashtable.bucket[ length ];
      this.loadsize = ( int ) ( ( double ) this.loadFactor * ( double ) length );
      this.isWriterInProgress = false;
    }

    public Hashtable( int capacity, float loadFactor, IEqualityComparer equalityComparer ) : this( capacity, loadFactor ) {
      this._keycomparer = equalityComparer;
    }

    public Hashtable( IEqualityComparer equalityComparer ) : this( 0, 1f, equalityComparer ) { }

    public Hashtable( int capacity, IEqualityComparer equalityComparer ) : this( capacity, 1f, equalityComparer ) { }

    public Hashtable( IDictionary d ) : this( d, 1f ) { }

    public Hashtable( IDictionary d, float loadFactor ) : this( d, loadFactor, ( IEqualityComparer ) null ) { }

    public Hashtable( IDictionary d, IEqualityComparer equalityComparer ) : this( d, 1f, equalityComparer ) { }

    public Hashtable( IDictionary d, float loadFactor, IEqualityComparer equalityComparer ) : this(
      d != null ? d.Count : 0, loadFactor, equalityComparer ) {
      if ( d == null ) {
        throw new ArgumentNullException( nameof( d ), "ArgumentNull_Dictionary" );
      }

      IDictionaryEnumerator enumerator = d.GetEnumerator();
      while ( enumerator.MoveNext() ) {
        this.Add( enumerator.Key, enumerator.Value );
      }
    }

    /*protected Hashtable( SerializationInfo info, StreamingContext context ) {
      HashHelpers.SerializationInfoTable.Add( ( object ) this, info );
    }*/

    private uint InitHash( object key, int hashsize, out uint seed, out uint incr ) {
      uint num = ( uint ) ( this.GetHash( key ) & int.MaxValue );
      seed = num;
      incr = 1U + seed * 101U % ( uint ) ( hashsize - 1 );
      return num;
    }

    public virtual void Add( object key, object value ) {
      this.Insert( key, value, true );
    }

    /// <summary>
    ///   Удаляет все элементы из коллекции <see cref="T:System.Collections.Hashtable" />.
    /// </summary>
    /// <exception cref="T:System.NotSupportedException">
    ///   Объект <see cref="T:System.Collections.Hashtable" /> доступен только для чтения.
    /// </exception>
    public virtual void Clear() {
      if ( this.count == 0 && this.occupancy == 0 ) {
        return;
      }

      this.isWriterInProgress = true;
      for ( int index = 0; index < this.buckets.Length; ++index ) {
        this.buckets[ index ].hash_coll = 0;
        this.buckets[ index ].key = ( object ) null;
        this.buckets[ index ].val = ( object ) null;
      }

      this.count = 0;
      this.occupancy = 0;
      this.UpdateVersion();
      this.isWriterInProgress = false;
    }

    /// <summary>
    /// Clone this instance.
    /// </summary>
    public virtual object Clone() {
      Hashtable.bucket[] buckets = this.buckets;
      Hashtable hashtable = new Hashtable( this.count, this._keycomparer );
      hashtable.version = this.version;
      hashtable.loadFactor = this.loadFactor;
      hashtable.count = 0;
      int length = buckets.Length;
      while ( length > 0 ) {
        --length;
        object key = buckets[ length ].key;
        if ( key != null && key != buckets ) {
          hashtable[ key ] = buckets[ length ].val;
        }
      }

      return ( object ) hashtable;
    }

    public virtual bool Contains( object key ) {
      return this.ContainsKey( key );
    }

    public virtual bool ContainsKey( object key ) {
      if ( key == null ) {
        throw new ArgumentNullException( nameof( key ), "ArgumentNull_Key" );
      }

      Hashtable.bucket[] buckets = this.buckets;
      uint seed;
      uint incr;
      uint num1 = this.InitHash( key, buckets.Length, out seed, out incr );
      int num2 = 0;
      int index = ( int ) ( seed % ( uint ) buckets.Length );
      Hashtable.bucket bucket;
      do {
        bucket = buckets[ index ];
        if ( bucket.key == null ) {
          return false;
        }

        if ( ( long ) ( bucket.hash_coll & int.MaxValue ) == ( long ) num1 && this.KeyEquals( bucket.key, key ) ) {
          return true;
        }

        index = ( int ) ( ( ( long ) index + ( long ) incr ) % ( long ) ( uint ) buckets.Length );
      } while ( bucket.hash_coll < 0 && ++num2 < buckets.Length );

      return false;
    }

    public virtual bool ContainsValue( object value ) {
      if ( value == null ) {
        int length = this.buckets.Length;
        while ( --length >= 0 ) {
          if ( this.buckets[ length ].key != null && this.buckets[ length ].key != this.buckets &&
               this.buckets[ length ].val == null ) {
            return true;
          }
        }
      } else {
        int length = this.buckets.Length;
        while ( --length >= 0 ) {
          object val = this.buckets[ length ].val;
          if ( val != null && val.Equals( value ) ) {
            return true;
          }
        }
      }

      return false;
    }

    private void CopyKeys( Array array, int arrayIndex ) {
      Hashtable.bucket[] buckets = this.buckets;
      int length = buckets.Length;
      while ( --length >= 0 ) {
        object key = buckets[ length ].key;
        if ( key != null && key != this.buckets ) {
          array.SetValue( key, arrayIndex++ );
        }
      }
    }

    private void CopyEntries( Array array, int arrayIndex ) {
      Hashtable.bucket[] buckets = this.buckets;
      int length = buckets.Length;
      while ( --length >= 0 ) {
        object key = buckets[ length ].key;
        if ( key != null && key != this.buckets ) {
          DictionaryEntry dictionaryEntry = new DictionaryEntry( key, buckets[ length ].val );
          array.SetValue( ( object ) dictionaryEntry, arrayIndex++ );
        }
      }
    }

    public virtual void CopyTo( Array array, int arrayIndex ) {
      if ( array == null ) {
        throw new ArgumentNullException( nameof( array ), "ArgumentNull_Array" );
      }

      if ( array.Rank != 1 ) {
        throw new ArgumentException( "Arg_RankMultiDimNotSupported" );
      }

      if ( arrayIndex < 0 ) {
        throw new ArgumentOutOfRangeException( nameof( arrayIndex ), "ArgumentOutOfRange_NeedNonNegNum" );
      }

      if ( array.Length - arrayIndex < this.Count ) {
        throw new ArgumentException( "Arg_ArrayPlusOffTooSmall" );
      }

      this.CopyEntries( array, arrayIndex );
    }

    internal virtual KeyValuePairs[] ToKeyValuePairsArray() {
      KeyValuePairs[] keyValuePairsArray = new KeyValuePairs[ this.count ];
      int num = 0;
      Hashtable.bucket[] buckets = this.buckets;
      int length = buckets.Length;
      while ( --length >= 0 ) {
        object key = buckets[ length ].key;
        if ( key != null && key != this.buckets ) {
          keyValuePairsArray[ num++ ] = new KeyValuePairs( key, buckets[ length ].val );
        }
      }

      return keyValuePairsArray;
    }

    private void CopyValues( Array array, int arrayIndex ) {
      Hashtable.bucket[] buckets = this.buckets;
      int length = buckets.Length;
      while ( --length >= 0 ) {
        object key = buckets[ length ].key;
        if ( key != null && key != this.buckets ) {
          array.SetValue( buckets[ length ].val, arrayIndex++ );
        }
      }
    }

    public virtual object this[ object key ] {
      get {
        if ( key == null ) {
          throw new ArgumentNullException( nameof( key ), Environment.GetResourceString( "ArgumentNull_Key" ) );
        }

        Hashtable.bucket[] buckets = this.buckets;
        uint seed;
        uint incr;
        uint num1 = this.InitHash( key, buckets.Length, out seed, out incr );
        int num2 = 0;
        int index = ( int ) ( seed % ( uint ) buckets.Length );
        Hashtable.bucket bucket;
        do {
          int num3 = 0;
          int version;
          do {
            version = this.version;
            bucket = buckets[ index ];
            if ( ++num3 % 8 == 0 ) {
              Thread.Sleep( 1 );
            }
          } while ( this.isWriterInProgress || version != this.version );

          if ( bucket.key == null ) {
            return ( object ) null;
          }

          if ( ( long ) ( bucket.hash_coll & int.MaxValue ) == ( long ) num1 && this.KeyEquals( bucket.key, key ) ) {
            return bucket.val;
          }

          index = ( int ) ( ( ( long ) index + ( long ) incr ) % ( long ) ( uint ) buckets.Length );
        } while ( bucket.hash_coll < 0 && ++num2 < buckets.Length );

        return ( object ) null;
      }
      set => this.Insert( key, value, false );
    }

    private void Expand()
    {
        Rehash(HashHelpers.ExpandPrime(this.buckets.Length), false);
    }

    private void Rehash()
    {
        this.Rehash(this.buckets.Length, false);
    }

    private void UpdateVersion()
    {
        ++this.version;
    }

    private void Rehash( int newsize, bool forceNewHashCode ) {
      this.occupancy = 0;
      Hashtable.bucket[] newBuckets = new Hashtable.bucket[ newsize ];
      for ( int index = 0; index < this.buckets.Length; ++index ) {
        Hashtable.bucket bucket = this.buckets[ index ];
        if ( bucket.key != null && bucket.key != this.buckets ) {
          int hashcode = ( forceNewHashCode ? this.GetHash( bucket.key ) : bucket.hash_coll ) & int.MaxValue;
          this.putEntry( newBuckets, bucket.key, bucket.val, hashcode );
        }
      }

      this.isWriterInProgress = true;
      this.buckets = newBuckets;
      this.loadsize = ( int ) ( ( double ) this.loadFactor * ( double ) newsize );
      this.UpdateVersion();
      this.isWriterInProgress = false;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return ( IEnumerator ) new Hashtable.HashtableEnumerator( this, 3 );
    }

    public virtual IDictionaryEnumerator GetEnumerator() {
      return ( IDictionaryEnumerator ) new Hashtable.HashtableEnumerator( this, 3 );
    }

    protected virtual int GetHash( object key ) {
      if ( this._keycomparer != null ) {
        return this._keycomparer.GetHashCode( key );
      }

      return key.GetHashCode();
    }

    public virtual bool IsReadOnly => false;

    public virtual bool IsFixedSize => false;

    public virtual bool IsSynchronized => false;

    protected virtual bool KeyEquals( object item, object key ) {
      if ( this.buckets == item ) {
        return false;
      }

      if ( item == key ) {
        return true;
      }

      if ( this._keycomparer != null ) {
        return this._keycomparer.Equals( item, key );
      }

      if ( item != null ) {
        return item.Equals( key );
      }

      return false;
    }

    public virtual ICollection Keys => this.keys ?? ( this.keys = ( ICollection ) new Hashtable.KeyCollection( this ) );

    public virtual ICollection Values => this.values ?? ( this.values = ( ICollection ) new Hashtable.ValueCollection( this ) );

    private void Insert( object key, object nvalue, bool add ) {
      if ( key == null ) {
        throw new ArgumentNullException( nameof( key ), "ArgumentNull_Key" );
      }

      if ( this.count >= this.loadsize ) {
        this.Expand();
      } else if ( this.occupancy > this.loadsize && this.count > 100 ) {
        this.Rehash();
      }

      uint seed;
      uint incr;
      uint num1 = this.InitHash( key, this.buckets.Length, out seed, out incr );
      int num2 = 0;
      int index1 = -1;
      int index2 = ( int ) ( seed % ( uint ) this.buckets.Length );
      do {
        if ( index1 == -1 && this.buckets[ index2 ].key == this.buckets && this.buckets[ index2 ].hash_coll < 0 ) {
          index1 = index2;
        }

        if ( this.buckets[ index2 ].key == null || this.buckets[ index2 ].key == this.buckets &&
             ( ( long ) this.buckets[ index2 ].hash_coll & 2147483648L ) == 0L ) {
          if ( index1 != -1 ) {
            index2 = index1;
          }

          //Thread.BeginCriticalRegion();
          this.isWriterInProgress = true;
          this.buckets[ index2 ].val = nvalue;
          this.buckets[ index2 ].key = key;
          //Because bridge can't properly translate expression |= we need this fix:
          var hashColl = this.buckets[index2].hash_coll;
          this.buckets[ index2 ].hash_coll = hashColl | ( int ) num1;
          ++this.count;
          this.UpdateVersion();
          this.isWriterInProgress = false;
          //Thread.EndCriticalRegion();
          if ( num2 <= 100 || !HashHelpers.IsWellKnownEqualityComparer( ( object ) this._keycomparer ) ||
               this._keycomparer != null && this._keycomparer is RandomizedObjectEqualityComparer ) {
            return;
          }

          this._keycomparer = HashHelpers.GetRandomizedEqualityComparer( ( object ) this._keycomparer );
          this.Rehash( this.buckets.Length, true );
          return;
        }

        if ( ( long ) ( this.buckets[ index2 ].hash_coll & int.MaxValue ) == ( long ) num1 &&
             this.KeyEquals( this.buckets[ index2 ].key, key ) ) {
          if ( add ) {
            throw new ArgumentException( Environment.GetResourceString( "Argument_AddingDuplicate__",
              this.buckets[ index2 ].key, key ) );
          }

          //Thread.BeginCriticalRegion();
          this.isWriterInProgress = true;
          this.buckets[ index2 ].val = nvalue;
          this.UpdateVersion();
          this.isWriterInProgress = false;
          //Thread.EndCriticalRegion();
          if ( num2 <= 100 || !HashHelpers.IsWellKnownEqualityComparer( ( object ) this._keycomparer ) ||
               this._keycomparer != null && this._keycomparer is RandomizedObjectEqualityComparer ) {
            return;
          }

          this._keycomparer = HashHelpers.GetRandomizedEqualityComparer( ( object ) this._keycomparer );
          this.Rehash( this.buckets.Length, true );
          return;
        }

        if (index1 == -1 && this.buckets[index2].hash_coll >= 0) {
            var hashColl = this.buckets[index2].hash_coll;
            this.buckets[index2].hash_coll = hashColl | int.MinValue;
            ++this.occupancy;
        }

        index2 = ( int ) ( ( ( long ) index2 + ( long ) incr ) % ( long ) ( uint ) this.buckets.Length );
      } while ( ++num2 < this.buckets.Length );

      if ( index1 == -1 ) {
        throw new InvalidOperationException( "InvalidOperation_HashInsertFailed" );
      }

      //Thread.BeginCriticalRegion();
      this.isWriterInProgress = true;
      this.buckets[ index1 ].val = nvalue;
      this.buckets[ index1 ].key = key;
      var hash = this.buckets[index1].hash_coll;
      this.buckets[ index1 ].hash_coll = hash | ( int ) num1;
      ++this.count;
      this.UpdateVersion();
      this.isWriterInProgress = false;
      //Thread.EndCriticalRegion();
      if ( this.buckets.Length <= 100 || !HashHelpers.IsWellKnownEqualityComparer( ( object ) this._keycomparer ) ||
           this._keycomparer != null && this._keycomparer is RandomizedObjectEqualityComparer ) {
        return;
      }

      this._keycomparer = HashHelpers.GetRandomizedEqualityComparer( ( object ) this._keycomparer );
      this.Rehash( this.buckets.Length, true );
    }

    private void putEntry( Hashtable.bucket[] newBuckets, object key, object nvalue, int hashcode ) {
      uint num1 = ( uint ) hashcode;
      uint num2 = 1U + num1 * 101U % ( uint ) ( newBuckets.Length - 1 );
      int index;
      for ( index = ( int ) ( num1 % ( uint ) newBuckets.Length );
        newBuckets[ index ].key != null && newBuckets[ index ].key != this.buckets;
        index = ( int ) ( ( ( long ) index + ( long ) num2 ) % ( long ) ( uint ) newBuckets.Length ) ) {
          if ( newBuckets[index].hash_coll >= 0 ) {
              var hash_coll = newBuckets[ index ].hash_coll;
              newBuckets[ index ].hash_coll = hash_coll | int.MinValue;
              ++this.occupancy;
          }
      }

      newBuckets[ index ].val = nvalue;
      newBuckets[ index ].key = key;
      var hash = newBuckets[index].hash_coll;
      newBuckets[ index ].hash_coll = hash | hashcode;
    }

    public virtual void Remove( object key ) {
      if ( key == null ) {
        throw new ArgumentNullException( nameof( key ), "ArgumentNull_Key" );
      }

      uint seed;
      uint incr;
      uint num1 = this.InitHash( key, this.buckets.Length, out seed, out incr );
      int num2 = 0;
      int index = ( int ) ( seed % ( uint ) this.buckets.Length );
      Hashtable.bucket bucket;
      do {
        bucket = this.buckets[ index ];
        if ( ( long ) ( bucket.hash_coll & int.MaxValue ) == ( long ) num1 && this.KeyEquals( bucket.key, key ) ) {
          //Thread.BeginCriticalRegion();
          this.isWriterInProgress = true;
          this.buckets[ index ].hash_coll &= int.MinValue;
          this.buckets[ index ].key = this.buckets[ index ].hash_coll == 0 ? ( object ) null : ( object ) this.buckets;
          this.buckets[ index ].val = ( object ) null;
          --this.count;
          this.UpdateVersion();
          this.isWriterInProgress = false;
          //Thread.EndCriticalRegion();
          break;
        }

        index = ( int ) ( ( ( long ) index + ( long ) incr ) % ( long ) ( uint ) this.buckets.Length );
      } while ( bucket.hash_coll < 0 && ++num2 < this.buckets.Length );
    }

    public virtual object SyncRoot {
      get { return null; }
    }

    public virtual int Count => this.count;

    private struct bucket {
      public object key;
      public object val;
      public int hash_coll;
    }

    [ Serializable ]
    private class KeyCollection : ICollection, IEnumerable {
      private Hashtable _hashtable;

      internal KeyCollection( Hashtable hashtable ) {
        this._hashtable = hashtable;
      }

      public virtual void CopyTo( Array array, int arrayIndex ) {
        if ( array == null ) {
          throw new ArgumentNullException( nameof( array ) );
        }

        if ( array.Rank != 1 ) {
          throw new ArgumentException( "Arg_RankMultiDimNotSupported" );
        }

        if ( arrayIndex < 0 ) {
          throw new ArgumentOutOfRangeException( nameof( arrayIndex ), "ArgumentOutOfRange_NeedNonNegNum" );
        }

        if ( array.Length - arrayIndex < this._hashtable.count ) {
          throw new ArgumentException( "Arg_ArrayPlusOffTooSmall" );
        }

        this._hashtable.CopyKeys( array, arrayIndex );
      }

      public virtual IEnumerator GetEnumerator() {
        return ( IEnumerator ) new Hashtable.HashtableEnumerator( this._hashtable, 1 );
      }

      public virtual bool IsSynchronized => this._hashtable.IsSynchronized;

      public virtual object SyncRoot => this._hashtable.SyncRoot;

      public virtual int Count => this._hashtable.count;
    }

    [ Serializable ]
    private class ValueCollection : ICollection, IEnumerable {
      private Hashtable _hashtable;

      internal ValueCollection( Hashtable hashtable ) {
        this._hashtable = hashtable;
      }

      public virtual void CopyTo( Array array, int arrayIndex ) {
        if ( array == null ) {
          throw new ArgumentNullException( nameof( array ) );
        }

        if ( array.Rank != 1 ) {
          throw new ArgumentException( "Arg_RankMultiDimNotSupported" );
        }

        if ( arrayIndex < 0 ) {
          throw new ArgumentOutOfRangeException( nameof( arrayIndex ), "ArgumentOutOfRange_NeedNonNegNum" );
        }

        if ( array.Length - arrayIndex < this._hashtable.count ) {
          throw new ArgumentException( "Arg_ArrayPlusOffTooSmall" );
        }

        this._hashtable.CopyValues( array, arrayIndex );
      }

      public virtual IEnumerator GetEnumerator() {
        return ( IEnumerator ) new Hashtable.HashtableEnumerator( this._hashtable, 2 );
      }

      public virtual bool IsSynchronized => this._hashtable.IsSynchronized;

      public virtual object SyncRoot => this._hashtable.SyncRoot;

      public virtual int Count => this._hashtable.count;
    }


    [ Serializable ]
    private class HashtableEnumerator : IDictionaryEnumerator, IEnumerator, ICloneable {
      private Hashtable hashtable;
      private int bucket;
      private int version;
      private bool current;
      private int getObjectRetType;
      private object currentKey;
      private object currentValue;
      internal const int Keys = 1;
      internal const int Values = 2;
      internal const int DictEntry = 3;

      internal HashtableEnumerator( Hashtable hashtable, int getObjRetType ) {
        this.hashtable = hashtable;
        this.bucket = hashtable.buckets.Length;
        this.version = hashtable.version;
        this.current = false;
        this.getObjectRetType = getObjRetType;
      }

      public object Clone() {
        return this.MemberwiseClone();
      }

      public virtual object Key {
        get {
          if ( !this.current ) {
            throw new InvalidOperationException( "InvalidOperation_EnumNotStarted" );
          }

          return this.currentKey;
        }
      }

      public virtual bool MoveNext() {
        if ( this.version != this.hashtable.version ) {
          throw new InvalidOperationException( "InvalidOperation_EnumFailedVersion" );
        }

        while ( this.bucket > 0 ) {
          --this.bucket;
          object key = this.hashtable.buckets[ this.bucket ].key;
          if ( key != null && key != this.hashtable.buckets ) {
            this.currentKey = key;
            this.currentValue = this.hashtable.buckets[ this.bucket ].val;
            this.current = true;
            return true;
          }
        }

        this.current = false;
        return false;
      }

      public virtual DictionaryEntry Entry {
        get {
          if ( !this.current ) {
            throw new InvalidOperationException( "InvalidOperation_EnumOpCantHappen" );
          }

          return new DictionaryEntry( this.currentKey, this.currentValue );
        }
      }

      public virtual object Current {
        get {
          if ( !this.current ) {
            throw new InvalidOperationException( "InvalidOperation_EnumOpCantHappen" );
          }

          if ( this.getObjectRetType == 1 ) {
            return this.currentKey;
          }

          if ( this.getObjectRetType == 2 ) {
            return this.currentValue;
          }

          return ( object ) new DictionaryEntry( this.currentKey, this.currentValue );
        }
      }

      public virtual object Value {
        get {
          if ( !this.current ) {
            throw new InvalidOperationException( "InvalidOperation_EnumOpCantHappen" );
          }

          return this.currentValue;
        }
      }

      public virtual void Reset() {
        if ( this.version != this.hashtable.version ) {
          throw new InvalidOperationException( "InvalidOperation_EnumFailedVersion" );
        }

        this.current = false;
        this.bucket = this.hashtable.buckets.Length;
        this.currentKey = ( object ) null;
        this.currentValue = ( object ) null;
      }
    }

    internal class HashtableDebugView {
      private Hashtable hashtable;

      public HashtableDebugView( Hashtable hashtable ) {
        if ( hashtable == null ) {
          throw new ArgumentNullException( nameof( hashtable ) );
        }

        this.hashtable = hashtable;
      }

      public KeyValuePairs[] Items => this.hashtable.ToKeyValuePairsArray();
    }
  }
}

#pragma warning restore 169
#pragma warning restore 649
