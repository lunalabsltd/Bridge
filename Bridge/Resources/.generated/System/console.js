    /*System.Console start.*/
    Bridge.define("System.Console", {
        statics: {
            methods: {
                /*System.Console.Write:static start.*/
                Write: function (value) {
                    var con = Bridge.global.console;

                    if (con && con.log) {
                        con.log(!Bridge.isDefined(Bridge.unbox(value)) ? "" : Bridge.unbox(value));
                    }
                },
                /*System.Console.Write:static end.*/

                /*System.Console.WriteLine:static start.*/
                WriteLine: function (value) {
                    var con = Bridge.global.console;

                    if (con && con.log) {
                        con.log(!Bridge.isDefined(Bridge.unbox(value)) ? "" : Bridge.unbox(value));
                    }
                },
                /*System.Console.WriteLine:static end.*/

                /*System.Console.TransformChars:static start.*/
                TransformChars: function (buffer, all, index, count) {
                    if (all !== 1) {
                        if (buffer == null) {
                            throw new System.ArgumentNullException.$ctor1("buffer");
                        }

                        if (index < 0) {
                            throw new System.ArgumentOutOfRangeException.$ctor4("index", "less than zero");
                        }

                        if (count < 0) {
                            throw new System.ArgumentOutOfRangeException.$ctor4("count", "less than zero");
                        }

                        if (((index + count) | 0) > buffer.length) {
                            throw new System.ArgumentException.$ctor1("index plus count specify a position that is not within buffer.");
                        }
                    }

                    var s = "";
                    if (buffer != null) {
                        if (all === 1) {
                            index = 0;
                            count = buffer.length;
                        }

                        for (var i = index; i < ((index + count) | 0); i = (i + 1) | 0) {
                            s = (s || "") + String.fromCharCode(buffer[System.Array.index(i, buffer)]);
                        }
                    }

                    return s;
                },
                /*System.Console.TransformChars:static end.*/

                /*System.Console.Clear:static start.*/
                Clear: function () {
                    var con = Bridge.global.console;

                    if (con && con.clear) {
                        con.clear();
                    }
                },
                /*System.Console.Clear:static end.*/


            }
        }
    });
    /*System.Console end.*/
