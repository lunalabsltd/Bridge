namespace System.Globalization
{
    [Bridge.Enum(Bridge.Emit.Value)]
    public enum NumberStyles
    {
        None = 0,
        AllowLeadingWhite = 1,
        AllowTrailingWhite = 2,
        AllowLeadingSign = 4,
        AllowTrailingSign = 8,
        AllowParentheses = 16, // 0x00000010
        AllowDecimalPoint = 32, // 0x00000020
        AllowThousands = 64, // 0x00000040
        AllowExponent = 128, // 0x00000080
        AllowCurrencySymbol = 256, // 0x00000100
        AllowHexSpecifier = 512, // 0x00000200
        Integer = AllowLeadingSign | AllowTrailingWhite | AllowLeadingWhite, // 0x00000007
        HexNumber = AllowHexSpecifier | AllowTrailingWhite | AllowLeadingWhite, // 0x00000203
        Number = Integer | AllowThousands | AllowDecimalPoint | AllowTrailingSign, // 0x0000006F
        Float = Integer | AllowExponent | AllowDecimalPoint, // 0x000000A7
        Currency = Number | AllowCurrencySymbol | AllowParentheses, // 0x0000017F
        Any = Currency | AllowExponent, // 0x000001FF
    }
}