using System.Collections.Generic;

namespace CitizenMatt.DotPeek.AssemblyLists.Tests
{
    public class AssemblyListBuilder
    {
        private readonly IDictionary<string, IList<string>> assemblyLists;

        public static AssemblyListBuilder New()
        {
            return new AssemblyListBuilder();
        }

        private AssemblyListBuilder()
        {
            assemblyLists = new Dictionary<string, IList<string>>();
        }

        public AssemblyListBuilder With(string list, params string[] filenames)
        {
            assemblyLists.Add(list, new List<string>(filenames));
            return this;
        }

        public IDictionary<string, IList<string>> Done()
        {
            return assemblyLists;
        }
    }
}