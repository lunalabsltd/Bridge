Bridge.assembly("TestProject", function ($asm, globals) {
    "use strict";


    var $m = Bridge.setMetadata,
        $n = ["Test.BridgeIssues.N3712","System"];
    $m("Test.BridgeIssues.N3712.N3712", function () { return {"nested":[$n[0].N3712.ClassA],"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"}]}; }, $n);
    $m("Test.BridgeIssues.N3712.N3712.ClassA", function () { return {"td":$n[0].N3712,"nested":[$n[0].N3712.ClassA.Foo],"att":1048578,"a":2,"m":[{"a":2,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"SomeString","t":16,"rt":$n[1].String,"g":{"a":2,"n":"get_SomeString","t":8,"rt":$n[1].String,"fg":"SomeString"},"s":{"a":2,"n":"set_SomeString","t":8,"p":[$n[1].String],"rt":$n[1].Void,"fs":"SomeString"},"fn":"SomeString"}]}; }, $n);
    $m("Test.BridgeIssues.N3712.N3712.ClassA.Foo", function () { return {"td":$n[0].N3712.ClassA,"att":1048578,"a":2,"m":[{"a":2,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"MainController","t":8,"sn":"MainController","rt":$n[1].String},{"a":1,"n":"SomeStr","t":4,"rt":$n[1].String,"sn":"SomeStr","ro":true}]}; }, $n);
});
