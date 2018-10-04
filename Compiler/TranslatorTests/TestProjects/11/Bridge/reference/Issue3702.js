Bridge.assembly("TestProject", function ($asm, globals) {
    "use strict";

    Bridge.define("Issue3702.Person", {
        $metadata : function () { return {"att":1048577,"a":2,"m":[{"a":2,"isSynthetic":true,"n":".ctor","t":1,"sn":"ctor"},{"a":2,"n":"Name","t":16,"rt":System.String,"g":{"a":2,"n":"get_Name","t":8,"rt":System.String,"fg":"Name"},"s":{"a":2,"n":"set_Name","t":8,"p":[System.String],"rt":System.Void,"fs":"Name"},"fn":"Name"},{"a":2,"n":"Company","t":4,"rt":System.String,"sn":"Company"}]}; },
        fields: {
            Company: null
        },
        props: {
            /**
             * This should output as "Name?: string | null
             On the .ts file output.
             *
             * @instance
             * @public
             * @memberof Issue3702.Person
             * @function Name
             * @type string
             */
            Name: null
        }
    });
});
