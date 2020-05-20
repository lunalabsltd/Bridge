using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Serialization
{
    [ComVisible(true)]
    public sealed class SerializationInfo
    {
        private const int defaultSize = 4;
        private const string s_mscorlibAssemblySimpleName = "mscorlib";
        private const string s_mscorlibFileName = "mscorlib.dll";
        internal string[] m_members;
        internal object[] m_data;
        internal Type[] m_types;
        private Dictionary<string, int> m_nameToIndex;
        internal int m_currMember;
        internal IFormatterConverter m_converter;
        private string m_fullTypeName;
        private string m_assemName;
        private Type objectType;
        private bool isFullTypeNameSetExplicit;
        private bool isAssemblyNameSetExplicit;
        private bool requireSameTokenInPartialTrust;

        [CLSCompliant(false)]
        public SerializationInfo(Type type, IFormatterConverter converter)
            : this(type, converter, false)
        {
        }

        [CLSCompliant(false)]
        public SerializationInfo(
            Type type,
            IFormatterConverter converter,
            bool requireSameTokenInPartialTrust)
        {
            if ((object) type == null)
                throw new ArgumentNullException(nameof(type));
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));
            this.objectType = type;
            this.m_fullTypeName = type.FullName;
            /*this.m_assemName = type.Module.Assembly.FullName;*/
            this.m_members = new string[4];
            this.m_data = new object[4];
            this.m_types = new Type[4];
            this.m_nameToIndex = new Dictionary<string, int>();
            this.m_converter = converter;
            this.requireSameTokenInPartialTrust = requireSameTokenInPartialTrust;
        }

        public string FullTypeName
        {
            get { return this.m_fullTypeName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                this.m_fullTypeName = value;
                this.isFullTypeNameSetExplicit = true;
            }
        }

        public string AssemblyName
        {
            get { return this.m_assemName; }
            [SecuritySafeCritical]
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                if (this.requireSameTokenInPartialTrust)
                    SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.m_assemName, value);
                this.m_assemName = value;
                this.isAssemblyNameSetExplicit = true;
            }
        }

        [SecuritySafeCritical]
        public void SetType(Type type)
        {
            if ((object) type == null)
                throw new ArgumentNullException(nameof(type));
            if (this.requireSameTokenInPartialTrust)
                SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.ObjectType.Assembly.FullName,
                    type.Assembly.FullName);
            if ((object) this.objectType == (object) type)
                return;
            this.objectType = type;
            this.m_fullTypeName = type.FullName;
            /*this.m_assemName = type.Module.Assembly.FullName;*/
            this.isFullTypeNameSetExplicit = false;
            this.isAssemblyNameSetExplicit = false;
        }

        private static bool Compare(byte[] a, byte[] b)
        {
            if (a == null || b == null || (a.Length == 0 || b.Length == 0) || a.Length != b.Length)
                return false;
            for (int index = 0; index < a.Length; ++index)
            {
                if ((int) a[index] != (int) b[index])
                    return false;
            }

            return true;
        }

        [SecuritySafeCritical]
        internal static void DemandForUnsafeAssemblyNameAssignments(
            string originalAssemblyName,
            string newAssemblyName)
        {
            /*if (SerializationInfo.IsAssemblyNameAssignmentSafe(originalAssemblyName, newAssemblyName))
                return;
            CodeAccessPermission.Demand(PermissionType.SecuritySerialization);*/
        }

        internal static bool IsAssemblyNameAssignmentSafe(
            string originalAssemblyName,
            string newAssemblyName)
        {
            if (originalAssemblyName == newAssemblyName)
                return true;
            System.Reflection.AssemblyName assemblyName1 = new System.Reflection.AssemblyName(originalAssemblyName);
            System.Reflection.AssemblyName assemblyName2 = new System.Reflection.AssemblyName(newAssemblyName);
            /*if (string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                return false;
            return SerializationInfo.Compare(assemblyName1.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());*/
            return default(bool);
        }

        public int MemberCount
        {
            get { return this.m_currMember; }
        }

        public Type ObjectType
        {
            get { return this.objectType; }
        }

        public bool IsFullTypeNameSetExplicit
        {
            get { return this.isFullTypeNameSetExplicit; }
        }

        public bool IsAssemblyNameSetExplicit
        {
            get { return this.isAssemblyNameSetExplicit; }
        }

        public SerializationInfoEnumerator GetEnumerator()
        {
            return new SerializationInfoEnumerator(this.m_members, this.m_data, this.m_types, this.m_currMember);
        }

        private void ExpandArrays()
        {
            int length = this.m_currMember * 2;
            if (length < this.m_currMember && int.MaxValue > this.m_currMember)
                length = int.MaxValue;
            string[] strArray = new string[length];
            object[] objArray = new object[length];
            Type[] typeArray = new Type[length];
            Array.Copy((Array) this.m_members, (Array) strArray, this.m_currMember);
            Array.Copy((Array) this.m_data, (Array) objArray, this.m_currMember);
            Array.Copy((Array) this.m_types, (Array) typeArray, this.m_currMember);
            this.m_members = strArray;
            this.m_data = objArray;
            this.m_types = typeArray;
        }

        public void AddValue(string name, object value, Type type)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if ((object) type == null)
                throw new ArgumentNullException(nameof(type));
            this.AddValueInternal(name, value, type);
        }

        public void AddValue(string name, object value)
        {
            if (value == null)
                this.AddValue(name, value, typeof(object));
            else
                this.AddValue(name, value, value.GetType());
        }

        public void AddValue(string name, bool value)
        {
            this.AddValue(name, (object) value, typeof(bool));
        }

        public void AddValue(string name, char value)
        {
            this.AddValue(name, (object) value, typeof(char));
        }

        [CLSCompliant(false)]
        public void AddValue(string name, sbyte value)
        {
            this.AddValue(name, (object) value, typeof(sbyte));
        }

        public void AddValue(string name, byte value)
        {
            this.AddValue(name, (object) value, typeof(byte));
        }

        public void AddValue(string name, short value)
        {
            this.AddValue(name, (object) value, typeof(short));
        }

        [CLSCompliant(false)]
        public void AddValue(string name, ushort value)
        {
            this.AddValue(name, (object) value, typeof(ushort));
        }

        public void AddValue(string name, int value)
        {
            this.AddValue(name, (object) value, typeof(int));
        }

        [CLSCompliant(false)]
        public void AddValue(string name, uint value)
        {
            this.AddValue(name, (object) value, typeof(uint));
        }

        public void AddValue(string name, long value)
        {
            this.AddValue(name, (object) value, typeof(long));
        }

        [CLSCompliant(false)]
        public void AddValue(string name, ulong value)
        {
            this.AddValue(name, (object) value, typeof(ulong));
        }

        public void AddValue(string name, float value)
        {
            this.AddValue(name, (object) value, typeof(float));
        }

        public void AddValue(string name, double value)
        {
            this.AddValue(name, (object) value, typeof(double));
        }

        public void AddValue(string name, Decimal value)
        {
            this.AddValue(name, (object) value, typeof(Decimal));
        }

        public void AddValue(string name, DateTime value)
        {
            this.AddValue(name, (object) value, typeof(DateTime));
        }

        internal void AddValueInternal(string name, object value, Type type)
        {
            if (this.m_nameToIndex.ContainsKey(name))
                throw new SerializationException(Environment.GetResourceString("Serialization_SameNameTwice"));
            this.m_nameToIndex.Add(name, this.m_currMember);
            if (this.m_currMember >= this.m_members.Length)
                this.ExpandArrays();
            this.m_members[this.m_currMember] = name;
            this.m_data[this.m_currMember] = value;
            this.m_types[this.m_currMember] = type;
            ++this.m_currMember;
        }

        internal void UpdateValue(string name, object value, Type type)
        {
            int element = this.FindElement(name);
            if (element < 0)
            {
                this.AddValueInternal(name, value, type);
            }
            else
            {
                this.m_data[element] = value;
                this.m_types[element] = type;
            }
        }

        private int FindElement(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            int num;
            if (this.m_nameToIndex.TryGetValue(name, out num))
                return num;
            return -1;
        }

        private object GetElement(string name, out Type foundType)
        {
            int element = this.FindElement(name);
            if (element == -1)
                throw new SerializationException(Environment.GetResourceString("Serialization_NotFound",
                    (object) name));
            foundType = this.m_types[element];
            return this.m_data[element];
        }

        [ComVisible(true)]
        private object GetElementNoThrow(string name, out Type foundType)
        {
            int element = this.FindElement(name);
            if (element == -1)
            {
                foundType = (Type) null;
                return (object) null;
            }

            foundType = this.m_types[element];
            return this.m_data[element];
        }

        /// <summary>
        ///   Извлекает значение из <see cref="T:System.Runtime.Serialization.SerializationInfo" /> хранения.
        /// </summary>
        /// <param name="name">Имя, связанное с извлекаемого значения.</param>
        /// <param name="type">
        ///   <see cref="T:System.Type" /> Извлекаемого значения.
        ///    Если сохраненное значение невозможно преобразовать в этот тип, система выдает <see cref="T:System.InvalidCastException" />.
        /// </param>
        /// <returns>
        ///   Объект заданного <see cref="T:System.Type" /> связанных с <paramref name="name" />.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   Параметр <paramref name="name" /> или <paramref name="type" /> имеет значение <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.InvalidCastException">
        ///   Значение, связанное с <paramref name="name" /> невозможно преобразовать в <paramref name="type" />.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///   Элемент с указанным именем не найден в текущем экземпляре.
        /// </exception>
        [SecuritySafeCritical]
        public object GetValue(string name, Type type)
        {
            if ((object) type == null)
                throw new ArgumentNullException(nameof(type));
            /*RuntimeType castType = type as RuntimeType;*/
            /*if (castType == (RuntimeType) null)
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"));
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if (RemotingServices.IsTransparentProxy(element))
            {
                if (RemotingServices.ProxyCheckCast(RemotingServices.GetRealProxy(element), castType))
                    return element;
            }
            else if ((object) foundType == (object) type || type.IsAssignableFrom(foundType) || element == null)
                return element;*/

            /*return this.m_converter.Convert(element, type);*/
            return default(object);
        }

        [SecuritySafeCritical]
        [ComVisible(true)]
        internal object GetValueNoThrow(string name, Type type)
        {
            Type foundType;
            object elementNoThrow = this.GetElementNoThrow(name, out foundType);
            if (elementNoThrow == null)
                return (object) null;
            /*if (RemotingServices.IsTransparentProxy(elementNoThrow))
            {
                if (RemotingServices.ProxyCheckCast(RemotingServices.GetRealProxy(elementNoThrow), (RuntimeType) type))
                    return elementNoThrow;
            }*/
            else if ((object) foundType == (object) type || type.IsAssignableFrom(foundType) || elementNoThrow == null)
                return elementNoThrow;

            return this.m_converter.Convert(elementNoThrow, type);
        }

        public bool GetBoolean(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(bool))
                return (bool) element;
            return this.m_converter.ToBoolean(element);
        }

        public char GetChar(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(char))
                return (char) element;
            return this.m_converter.ToChar(element);
        }

        [CLSCompliant(false)]
        public sbyte GetSByte(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(sbyte))
                return (sbyte) element;
            return this.m_converter.ToSByte(element);
        }

        public byte GetByte(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(byte))
                return (byte) element;
            return this.m_converter.ToByte(element);
        }

        public short GetInt16(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(short))
                return (short) element;
            return this.m_converter.ToInt16(element);
        }

        [CLSCompliant(false)]
        public ushort GetUInt16(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(ushort))
                return (ushort) element;
            return this.m_converter.ToUInt16(element);
        }

        public int GetInt32(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(int))
                return (int) element;
            return this.m_converter.ToInt32(element);
        }

        [CLSCompliant(false)]
        public uint GetUInt32(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(uint))
                return (uint) element;
            return this.m_converter.ToUInt32(element);
        }

        public long GetInt64(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(long))
                return (long) element;
            return this.m_converter.ToInt64(element);
        }

        [CLSCompliant(false)]
        public ulong GetUInt64(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(ulong))
                return (ulong) element;
            return this.m_converter.ToUInt64(element);
        }

        public float GetSingle(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(float))
                return (float) element;
            return this.m_converter.ToSingle(element);
        }

        public double GetDouble(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(double))
                return (double) element;
            return this.m_converter.ToDouble(element);
        }

        public Decimal GetDecimal(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(Decimal))
                return (Decimal) element;
            return this.m_converter.ToDecimal(element);
        }

        public DateTime GetDateTime(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(DateTime))
                return (DateTime) element;
            return this.m_converter.ToDateTime(element);
        }

        public string GetString(string name)
        {
            Type foundType;
            object element = this.GetElement(name, out foundType);
            if ((object) foundType == (object) typeof(string) || element == null)
                return (string) element;
            return this.m_converter.ToString(element);
        }

        internal string[] MemberNames
        {
            get { return this.m_members; }
        }

        internal object[] MemberValues
        {
            get { return this.m_data; }
        }
    }
}