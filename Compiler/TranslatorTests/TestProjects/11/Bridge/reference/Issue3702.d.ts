/// <reference path="./bridge.d.ts" />

declare namespace Issue3702 {
    interface Person {
        Company: string | null;
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
        Name?: string | null;
    }
    interface PersonFunc extends Function {
        prototype: Person;
        new (): Person;
    }
    var Person: PersonFunc;

}
