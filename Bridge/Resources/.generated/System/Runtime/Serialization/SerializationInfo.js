    /*System.Runtime.Serialization.SerializationInfo start.*/
    Bridge.define("System.Runtime.Serialization.SerializationInfo", {
        statics: {
            fields: {
                defaultSize: 0,
                s_mscorlibAssemblySimpleName: null,
                s_mscorlibFileName: null
            },
            ctors: {
                init: function () {
                    this.defaultSize = 4;
                    this.s_mscorlibAssemblySimpleName = "mscorlib";
                    this.s_mscorlibFileName = "mscorlib.dll";
                }
            },
            methods: {
                /*System.Runtime.Serialization.SerializationInfo.Compare:static start.*/
                Compare: function (a, b) {
                    if (a == null || b == null || (a.length === 0 || b.length === 0) || a.length !== b.length) {
                        return false;
                    }
                    for (var index = 0; index < a.length; index = (index + 1) | 0) {
                        if (a[System.Array.index(index, a)] !== b[System.Array.index(index, b)]) {
                            return false;
                        }
                    }

                    return true;
                },
                /*System.Runtime.Serialization.SerializationInfo.Compare:static end.*/

                /*System.Runtime.Serialization.SerializationInfo.DemandForUnsafeAssemblyNameAssignments:static start.*/
                DemandForUnsafeAssemblyNameAssignments: function (originalAssemblyName, newAssemblyName) {
                    /* if (SerializationInfo.IsAssemblyNameAssignmentSafe(originalAssemblyName, newAssemblyName))
                       return;
                    CodeAccessPermission.Demand(PermissionType.SecuritySerialization);*/
                },
                /*System.Runtime.Serialization.SerializationInfo.DemandForUnsafeAssemblyNameAssignments:static end.*/

                /*System.Runtime.Serialization.SerializationInfo.IsAssemblyNameAssignmentSafe:static start.*/
                IsAssemblyNameAssignmentSafe: function (originalAssemblyName, newAssemblyName) {
                    if (Bridge.referenceEquals(originalAssemblyName, newAssemblyName)) {
                        return true;
                    }
                    var assemblyName1 = new System.Reflection.AssemblyName(originalAssemblyName);
                    var assemblyName2 = new System.Reflection.AssemblyName(newAssemblyName);
                    /* if (string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                       return false;
                    return SerializationInfo.Compare(assemblyName1.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());*/
                    return Bridge.getDefaultValue(System.Boolean);
                },
                /*System.Runtime.Serialization.SerializationInfo.IsAssemblyNameAssignmentSafe:static end.*/


            }
        },
        fields: {
            m_members: null,
            m_data: null,
            m_types: null,
            m_nameToIndex: null,
            m_currMember: 0,
            m_converter: null,
            m_fullTypeName: null,
            m_assemName: null,
            objectType: null,
            isFullTypeNameSetExplicit: false,
            isAssemblyNameSetExplicit: false,
            requireSameTokenInPartialTrust: false
        },
        props: {
            FullTypeName: {
                get: function () {
                    return this.m_fullTypeName;
                },
                set: function (value) {
                    if (value == null) {
                        throw new System.ArgumentNullException.$ctor1("value");
                    }
                    this.m_fullTypeName = value;
                    this.isFullTypeNameSetExplicit = true;
                }
            },
            AssemblyName: {
                get: function () {
                    return this.m_assemName;
                },
                set: function (value) {
                    if (value == null) {
                        throw new System.ArgumentNullException.$ctor1("value");
                    }
                    if (this.requireSameTokenInPartialTrust) {
                        System.Runtime.Serialization.SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.m_assemName, value);
                    }
                    this.m_assemName = value;
                    this.isAssemblyNameSetExplicit = true;
                }
            },
            MemberCount: {
                get: function () {
                    return this.m_currMember;
                }
            },
            ObjectType: {
                get: function () {
                    return this.objectType;
                }
            },
            IsFullTypeNameSetExplicit: {
                get: function () {
                    return this.isFullTypeNameSetExplicit;
                }
            },
            IsAssemblyNameSetExplicit: {
                get: function () {
                    return this.isAssemblyNameSetExplicit;
                }
            },
            MemberNames: {
                get: function () {
                    return this.m_members;
                }
            },
            MemberValues: {
                get: function () {
                    return this.m_data;
                }
            }
        },
        ctors: {
            ctor: function (type, converter) {
                System.Runtime.Serialization.SerializationInfo.$ctor1.call(this, type, converter, false);
            },
            $ctor1: function (type, converter, requireSameTokenInPartialTrust) {
                this.$initialize();
                if (type == null) {
                    throw new System.ArgumentNullException.$ctor1("type");
                }
                if (converter == null) {
                    throw new System.ArgumentNullException.$ctor1("converter");
                }
                this.objectType = type;
                this.m_fullTypeName = Bridge.Reflection.getTypeFullName(type);
                /* this.m_assemName = type.Module.Assembly.FullName;*/
                this.m_members = System.Array.init(4, null, System.String);
                this.m_data = System.Array.init(4, null, System.Object);
                this.m_types = System.Array.init(4, null, System.Type);
                this.m_nameToIndex = new (System.Collections.Generic.Dictionary$2(System.String,System.Int32)).ctor();
                this.m_converter = converter;
                this.requireSameTokenInPartialTrust = requireSameTokenInPartialTrust;
            }
        },
        methods: {
            /*System.Runtime.Serialization.SerializationInfo.SetType start.*/
            SetType: function (type) {
                if (type == null) {
                    throw new System.ArgumentNullException.$ctor1("type");
                }
                if (this.requireSameTokenInPartialTrust) {
                    System.Runtime.Serialization.SerializationInfo.DemandForUnsafeAssemblyNameAssignments(Bridge.Reflection.getTypeAssembly(this.ObjectType).name, Bridge.Reflection.getTypeAssembly(type).name);
                }
                if (Bridge.referenceEquals(this.objectType, type)) {
                    return;
                }
                this.objectType = type;
                this.m_fullTypeName = Bridge.Reflection.getTypeFullName(type);
                /* this.m_assemName = type.Module.Assembly.FullName;*/
                this.isFullTypeNameSetExplicit = false;
                this.isAssemblyNameSetExplicit = false;
            },
            /*System.Runtime.Serialization.SerializationInfo.SetType end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetEnumerator start.*/
            GetEnumerator: function () {
                return new System.Runtime.Serialization.SerializationInfoEnumerator(this.m_members, this.m_data, this.m_types, this.m_currMember);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetEnumerator end.*/

            /*System.Runtime.Serialization.SerializationInfo.ExpandArrays start.*/
            ExpandArrays: function () {
                var length = Bridge.Int.mul(this.m_currMember, 2);
                if (length < this.m_currMember && 2147483647 > this.m_currMember) {
                    length = 2147483647;
                }
                var strArray = System.Array.init(length, null, System.String);
                var objArray = System.Array.init(length, null, System.Object);
                var typeArray = System.Array.init(length, null, System.Type);
                System.Array.copy(Bridge.cast(this.m_members, Array), 0, Bridge.cast(strArray, Array), 0, this.m_currMember);
                System.Array.copy(Bridge.cast(this.m_data, Array), 0, Bridge.cast(objArray, Array), 0, this.m_currMember);
                System.Array.copy(Bridge.cast(this.m_types, Array), 0, Bridge.cast(typeArray, Array), 0, this.m_currMember);
                this.m_members = strArray;
                this.m_data = objArray;
                this.m_types = typeArray;
            },
            /*System.Runtime.Serialization.SerializationInfo.ExpandArrays end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$10 start.*/
            AddValue$10: function (name, value, type) {
                if (name == null) {
                    throw new System.ArgumentNullException.$ctor1("name");
                }
                if (type == null) {
                    throw new System.ArgumentNullException.$ctor1("type");
                }
                this.AddValueInternal(name, value, type);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$10 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$9 start.*/
            AddValue$9: function (name, value) {
                if (value == null) {
                    this.AddValue$10(name, value, System.Object);
                } else {
                    this.AddValue$10(name, value, Bridge.getType(value));
                }
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$9 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue start.*/
            AddValue: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Boolean, System.Boolean.toString), System.Boolean);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$2 start.*/
            AddValue$2: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Char, String.fromCharCode, System.Char.getHashCode), System.Char);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$2 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$11 start.*/
            AddValue$11: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.SByte), System.SByte);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$11 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$1 start.*/
            AddValue$1: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Byte), System.Byte);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$1 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$6 start.*/
            AddValue$6: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Int16), System.Int16);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$6 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$13 start.*/
            AddValue$13: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.UInt16), System.UInt16);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$13 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$7 start.*/
            AddValue$7: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Int32), System.Int32);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$7 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$14 start.*/
            AddValue$14: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.UInt32), System.UInt32);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$14 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$8 start.*/
            AddValue$8: function (name, value) {
                this.AddValue$10(name, value, System.Int64);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$8 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$15 start.*/
            AddValue$15: function (name, value) {
                this.AddValue$10(name, value, System.UInt64);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$15 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$12 start.*/
            AddValue$12: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Single, System.Single.format, System.Single.getHashCode), System.Single);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$12 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$5 start.*/
            AddValue$5: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.Double, System.Double.format, System.Double.getHashCode), System.Double);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$5 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$4 start.*/
            AddValue$4: function (name, value) {
                this.AddValue$10(name, value, System.Decimal);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$4 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValue$3 start.*/
            AddValue$3: function (name, value) {
                this.AddValue$10(name, Bridge.box(value, System.DateTime, System.DateTime.format), System.DateTime);
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValue$3 end.*/

            /*System.Runtime.Serialization.SerializationInfo.AddValueInternal start.*/
            AddValueInternal: function (name, value, type) {
                if (this.m_nameToIndex.containsKey(name)) {
                    throw new System.Runtime.Serialization.SerializationException.$ctor1(System.Environment.GetResourceString("Serialization_SameNameTwice"));
                }
                this.m_nameToIndex.add(name, this.m_currMember);
                if (this.m_currMember >= this.m_members.length) {
                    this.ExpandArrays();
                }
                this.m_members[System.Array.index(this.m_currMember, this.m_members)] = name;
                this.m_data[System.Array.index(this.m_currMember, this.m_data)] = value;
                this.m_types[System.Array.index(this.m_currMember, this.m_types)] = type;
                this.m_currMember = (this.m_currMember + 1) | 0;
            },
            /*System.Runtime.Serialization.SerializationInfo.AddValueInternal end.*/

            /*System.Runtime.Serialization.SerializationInfo.UpdateValue start.*/
            UpdateValue: function (name, value, type) {
                var element = this.FindElement(name);
                if (element < 0) {
                    this.AddValueInternal(name, value, type);
                } else {
                    this.m_data[System.Array.index(element, this.m_data)] = value;
                    this.m_types[System.Array.index(element, this.m_types)] = type;
                }
            },
            /*System.Runtime.Serialization.SerializationInfo.UpdateValue end.*/

            /*System.Runtime.Serialization.SerializationInfo.FindElement start.*/
            FindElement: function (name) {
                if (name == null) {
                    throw new System.ArgumentNullException.$ctor1("name");
                }
                var num = { };
                if (this.m_nameToIndex.tryGetValue(name, num)) {
                    return num.v;
                }
                return -1;
            },
            /*System.Runtime.Serialization.SerializationInfo.FindElement end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetElement start.*/
            GetElement: function (name, foundType) {
                var element = this.FindElement(name);
                if (element === -1) {
                    throw new System.Runtime.Serialization.SerializationException.$ctor1(System.Environment.GetResourceString$1("Serialization_NotFound", [name]));
                }
                foundType.v = this.m_types[System.Array.index(element, this.m_types)];
                return this.m_data[System.Array.index(element, this.m_data)];
            },
            /*System.Runtime.Serialization.SerializationInfo.GetElement end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetElementNoThrow start.*/
            GetElementNoThrow: function (name, foundType) {
                var element = this.FindElement(name);
                if (element === -1) {
                    foundType.v = null;
                    return null;
                }

                foundType.v = this.m_types[System.Array.index(element, this.m_types)];
                return this.m_data[System.Array.index(element, this.m_data)];
            },
            /*System.Runtime.Serialization.SerializationInfo.GetElementNoThrow end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetValue start.*/
            GetValue: function (name, type) {
                if (type == null) {
                    throw new System.ArgumentNullException.$ctor1("type");
                }
                /* RuntimeType castType = type as RuntimeType;*/
                /* if (castType == (RuntimeType) null)
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

                /* return this.m_converter.Convert(element, type);*/
                return null;
            },
            /*System.Runtime.Serialization.SerializationInfo.GetValue end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetValueNoThrow start.*/
            GetValueNoThrow: function (name, type) {
                var foundType = { };
                var elementNoThrow = this.GetElementNoThrow(name, foundType);
                if (elementNoThrow == null) {
                    return null;
                } else {
                    if (Bridge.referenceEquals(foundType.v, type) || Bridge.Reflection.isAssignableFrom(type, foundType.v) || elementNoThrow == null) {
                        return elementNoThrow;
                    }
                }

                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$Convert(elementNoThrow, type);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetValueNoThrow end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetBoolean start.*/
            GetBoolean: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Boolean)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Boolean), System.Boolean));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToBoolean(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetBoolean end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetChar start.*/
            GetChar: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Char)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Char), System.Char));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToChar(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetChar end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetSByte start.*/
            GetSByte: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.SByte)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.SByte), System.SByte));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToSByte(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetSByte end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetByte start.*/
            GetByte: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Byte)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Byte), System.Byte));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToByte(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetByte end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetInt16 start.*/
            GetInt16: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Int16)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Int16), System.Int16));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToInt16(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetInt16 end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetUInt16 start.*/
            GetUInt16: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.UInt16)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.UInt16), System.UInt16));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToUInt16(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetUInt16 end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetInt32 start.*/
            GetInt32: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Int32)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Int32), System.Int32));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToInt32(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetInt32 end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetUInt32 start.*/
            GetUInt32: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.UInt32)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.UInt32), System.UInt32));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToUInt32(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetUInt32 end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetInt64 start.*/
            GetInt64: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Int64)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Int64), System.Int64));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToInt64(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetInt64 end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetUInt64 start.*/
            GetUInt64: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.UInt64)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.UInt64), System.UInt64));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToUInt64(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetUInt64 end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetSingle start.*/
            GetSingle: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Single)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Single), System.Single));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToSingle(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetSingle end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetDouble start.*/
            GetDouble: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Double)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Double), System.Double));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToDouble(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetDouble end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetDecimal start.*/
            GetDecimal: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.Decimal)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.Decimal), System.Decimal));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToDecimal(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetDecimal end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetDateTime start.*/
            GetDateTime: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.DateTime)) {
                    return System.Nullable.getValue(Bridge.cast(Bridge.unbox(element, System.DateTime), System.DateTime));
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToDateTime(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetDateTime end.*/

            /*System.Runtime.Serialization.SerializationInfo.GetString start.*/
            GetString: function (name) {
                var foundType = { };
                var element = this.GetElement(name, foundType);
                if (Bridge.referenceEquals(foundType.v, System.String) || element == null) {
                    return Bridge.cast(element, System.String);
                }
                return this.m_converter.System$Runtime$Serialization$IFormatterConverter$ToString(element);
            },
            /*System.Runtime.Serialization.SerializationInfo.GetString end.*/


        },
        overloads: {
            "AddValue(string, object, Type)": "AddValue$10",
            "AddValue(string, object)": "AddValue$9",
            "AddValue(string, char)": "AddValue$2",
            "AddValue(string, sbyte)": "AddValue$11",
            "AddValue(string, byte)": "AddValue$1",
            "AddValue(string, short)": "AddValue$6",
            "AddValue(string, ushort)": "AddValue$13",
            "AddValue(string, int)": "AddValue$7",
            "AddValue(string, uint)": "AddValue$14",
            "AddValue(string, long)": "AddValue$8",
            "AddValue(string, ulong)": "AddValue$15",
            "AddValue(string, float)": "AddValue$12",
            "AddValue(string, double)": "AddValue$5",
            "AddValue(string, Decimal)": "AddValue$4",
            "AddValue(string, DateTime)": "AddValue$3"
        }
    });
    /*System.Runtime.Serialization.SerializationInfo end.*/
