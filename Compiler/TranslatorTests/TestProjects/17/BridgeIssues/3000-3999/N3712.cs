namespace Test.BridgeIssues.N3712
{
    /// <summary>
    /// Ensures source maps is correctly output where partial classes'
    /// subclasses implementations are included in project file before the main
    /// class' definitions.
    /// This file should be included in the .csproj file -after- the '.Foo'
    /// one.
    /// </summary>
    public partial class N3712
    {
        public partial class ClassA
        {
            public ClassA()
            {
                SomeString = "Some String";
            }

            public string SomeString { get; set; }
        }
    }
}
