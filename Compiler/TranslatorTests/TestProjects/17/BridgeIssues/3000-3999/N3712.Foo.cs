namespace Test.BridgeIssues.N3712
{
    /// <summary>
    /// Ensures source maps is correctly output where partial classes'
    /// subclasses implementations are included in project file before the main
    /// class' definitions.
    /// This file should be included in the project file before 'N3712.cs'.
    /// </summary>
    public partial class N3712
    {
        public partial class ClassA
        {
            public class Foo
            {
                private readonly string SomeStr;

                public Foo()
                {
                    SomeStr = "123";
                }

                public string MainController()
                {
                    return SomeStr;
                }
            }
        }

    }
}
