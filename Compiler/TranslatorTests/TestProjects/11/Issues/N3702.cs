using Bridge;

//#3702 - TypeScript output's [Optional] attribute.
namespace Issue3702
{
    public class Person
    {
        /// <summary>
        /// This should output as "Name?: string | null
        /// On the .ts file output.
        /// </summary>
        [Optional]
        public string Name { get; set; }

        public string Company;
    }
}
