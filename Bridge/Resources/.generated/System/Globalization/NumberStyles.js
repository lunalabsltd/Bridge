    /*System.Globalization.NumberStyles start.*/
    Bridge.define("System.Globalization.NumberStyles", {
        $kind: "enum",
        statics: {
            fields: {
                None: 0,
                AllowLeadingWhite: 1,
                AllowTrailingWhite: 2,
                AllowLeadingSign: 4,
                Integer: 7,
                AllowTrailingSign: 8,
                AllowParentheses: 16,
                AllowDecimalPoint: 32,
                AllowThousands: 64,
                Number: 111,
                AllowExponent: 128,
                Float: 167,
                AllowCurrencySymbol: 256,
                Currency: 383,
                Any: 511,
                AllowHexSpecifier: 512,
                HexNumber: 515
            }
        },
        $flags: true
    });
    /*System.Globalization.NumberStyles end.*/
