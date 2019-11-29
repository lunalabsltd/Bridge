    /*System.IO.Iterator$1 start.*/
    Bridge.define("System.IO.Iterator$1", function (TSource) { return {
        inherits: [System.Collections.Generic.IEnumerable$1(TSource),System.Collections.Generic.IEnumerator$1(TSource)],
        fields: {
            state: 0,
            current: Bridge.getDefaultValue(TSource)
        },
        props: {
            Current: {
                get: function () {
                    return this.current;
                }
            },
            System$Collections$IEnumerator$Current: {
                get: function () {
                    return this.Current;
                }
            }
        },
        alias: [
            "Current", ["System$Collections$Generic$IEnumerator$1$" + Bridge.getTypeAlias(TSource) + "$Current$1", "System$Collections$Generic$IEnumerator$1$Current$1"],
            "Dispose", "System$IDisposable$Dispose",
            "GetEnumerator", ["System$Collections$Generic$IEnumerable$1$" + Bridge.getTypeAlias(TSource) + "$GetEnumerator", "System$Collections$Generic$IEnumerable$1$GetEnumerator"]
        ],
        ctors: {
            ctor: function () {
                this.$initialize();
            }
        },
        methods: {
            /*System.IO.Iterator$1.Dispose start.*/
            Dispose: function () {
                this.Dispose$1(true);
            },
            /*System.IO.Iterator$1.Dispose end.*/

            /*System.IO.Iterator$1.Dispose$1 start.*/
            Dispose$1: function (disposing) {
                this.current = Bridge.getDefaultValue(TSource);
                this.state = -1;
            },
            /*System.IO.Iterator$1.Dispose$1 end.*/

            /*System.IO.Iterator$1.GetEnumerator start.*/
            GetEnumerator: function () {
                if (this.state === 0) {
                    this.state = 1;
                    return this;
                }

                var duplicate = this.Clone();
                duplicate.state = 1;
                return duplicate;
            },
            /*System.IO.Iterator$1.GetEnumerator end.*/

            /*System.IO.Iterator$1.System$Collections$IEnumerable$GetEnumerator start.*/
            System$Collections$IEnumerable$GetEnumerator: function () {
                return this.GetEnumerator();
            },
            /*System.IO.Iterator$1.System$Collections$IEnumerable$GetEnumerator end.*/

            /*System.IO.Iterator$1.System$Collections$IEnumerator$reset start.*/
            System$Collections$IEnumerator$reset: function () {
                throw new System.NotSupportedException.ctor();
            },
            /*System.IO.Iterator$1.System$Collections$IEnumerator$reset end.*/


        }
    }; });
    /*System.IO.Iterator$1 end.*/
