/**
 * @compiler Bridge.NET 17.3.1
 */
Bridge.assembly("TestProject", function ($asm, globals) {
    "use strict";

    /** @namespace Test.BridgeIssues.N3712 */

    /**
     * Ensures source maps is correctly output where partial classes'
     subclasses implementations are included in project file before the main
     class' definitions.
     This file should be included in the project file before 'N3712.cs'.
     *
     * @public
     * @class Test.BridgeIssues.N3712.N3712
     */
    Bridge.define("Test.BridgeIssues.N3712.N3712");

    Bridge.define("Test.BridgeIssues.N3712.N3712.ClassA", {
        $kind: "nested class",
        props: {
            SomeString: null
        },
        ctors: {
            ctor: function () {
                this.$initialize();
                this.SomeString = "Some String";
            }
        }
    });

    Bridge.define("Test.BridgeIssues.N3712.N3712.ClassA.Foo", {
        $kind: "nested class",
        fields: {
            SomeStr: null
        },
        ctors: {
            ctor: function () {
                this.$initialize();
                this.SomeStr = "123";
            }
        },
        methods: {
            MainController: function () {
                return this.SomeStr;
            }
        }
    });
});

//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAiZmlsZSI6ICJUZXN0LkJyaWRnZUlzc3Vlcy5OMzcxMi5qcyIsCiAgInNvdXJjZVJvb3QiOiAiIiwKICAic291cmNlcyI6IFsiQnJpZGdlSXNzdWVzLzMwMDAtMzk5OS9OMzcxMi5jcyIsIkJyaWRnZUlzc3Vlcy8zMDAwLTM5OTkvTjM3MTIuRm9vLmNzIl0sCiAgIm5hbWVzIjogWyIiXSwKICAibWFwcGluZ3MiOiAiOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Z0JBZWdCQTs7Ozs7Ozs7Ozs7OztnQkNHSUE7Ozs7O2dCQUtBQSxPQUFPQSIsCiAgInNvdXJjZXNDb250ZW50IjogWyJuYW1lc3BhY2UgVGVzdC5CcmlkZ2VJc3N1ZXMuTjM3MTJcbntcbiAgICAvLy8gPHN1bW1hcnk+XG4gICAgLy8vIEVuc3VyZXMgc291cmNlIG1hcHMgaXMgY29ycmVjdGx5IG91dHB1dCB3aGVyZSBwYXJ0aWFsIGNsYXNzZXMnXG4gICAgLy8vIHN1YmNsYXNzZXMgaW1wbGVtZW50YXRpb25zIGFyZSBpbmNsdWRlZCBpbiBwcm9qZWN0IGZpbGUgYmVmb3JlIHRoZSBtYWluXG4gICAgLy8vIGNsYXNzJyBkZWZpbml0aW9ucy5cbiAgICAvLy8gVGhpcyBmaWxlIHNob3VsZCBiZSBpbmNsdWRlZCBpbiB0aGUgLmNzcHJvaiBmaWxlIC1hZnRlci0gdGhlICcuRm9vJ1xuICAgIC8vLyBvbmUuXG4gICAgLy8vIDwvc3VtbWFyeT5cbiAgICBwdWJsaWMgcGFydGlhbCBjbGFzcyBOMzcxMlxuICAgIHtcbiAgICAgICAgcHVibGljIHBhcnRpYWwgY2xhc3MgQ2xhc3NBXG4gICAgICAgIHtcbiAgICAgICAgICAgIHB1YmxpYyBDbGFzc0EoKVxuICAgICAgICAgICAge1xuICAgICAgICAgICAgICAgIFNvbWVTdHJpbmcgPSBcIlNvbWUgU3RyaW5nXCI7XG4gICAgICAgICAgICB9XG5cbiAgICAgICAgICAgIHB1YmxpYyBzdHJpbmcgU29tZVN0cmluZyB7IGdldDsgc2V0OyB9XG4gICAgICAgIH1cbiAgICB9XG59XG4iLCJuYW1lc3BhY2UgVGVzdC5CcmlkZ2VJc3N1ZXMuTjM3MTJcbntcbiAgICAvLy8gPHN1bW1hcnk+XG4gICAgLy8vIEVuc3VyZXMgc291cmNlIG1hcHMgaXMgY29ycmVjdGx5IG91dHB1dCB3aGVyZSBwYXJ0aWFsIGNsYXNzZXMnXG4gICAgLy8vIHN1YmNsYXNzZXMgaW1wbGVtZW50YXRpb25zIGFyZSBpbmNsdWRlZCBpbiBwcm9qZWN0IGZpbGUgYmVmb3JlIHRoZSBtYWluXG4gICAgLy8vIGNsYXNzJyBkZWZpbml0aW9ucy5cbiAgICAvLy8gVGhpcyBmaWxlIHNob3VsZCBiZSBpbmNsdWRlZCBpbiB0aGUgcHJvamVjdCBmaWxlIGJlZm9yZSAnTjM3MTIuY3MnLlxuICAgIC8vLyA8L3N1bW1hcnk+XG4gICAgcHVibGljIHBhcnRpYWwgY2xhc3MgTjM3MTJcbiAgICB7XG4gICAgICAgIHB1YmxpYyBwYXJ0aWFsIGNsYXNzIENsYXNzQVxuICAgICAgICB7XG4gICAgICAgICAgICBwdWJsaWMgY2xhc3MgRm9vXG4gICAgICAgICAgICB7XG4gICAgICAgICAgICAgICAgcHJpdmF0ZSByZWFkb25seSBzdHJpbmcgU29tZVN0cjtcblxuICAgICAgICAgICAgICAgIHB1YmxpYyBGb28oKVxuICAgICAgICAgICAgICAgIHtcbiAgICAgICAgICAgICAgICAgICAgU29tZVN0ciA9IFwiMTIzXCI7XG4gICAgICAgICAgICAgICAgfVxuXG4gICAgICAgICAgICAgICAgcHVibGljIHN0cmluZyBNYWluQ29udHJvbGxlcigpXG4gICAgICAgICAgICAgICAge1xuICAgICAgICAgICAgICAgICAgICByZXR1cm4gU29tZVN0cjtcbiAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICB9XG4gICAgICAgIH1cblxuICAgIH1cbn1cbiJdCn0K
