using System.Collections.Generic;
using System.Linq;

namespace CitizenMatt.DotPeek.AssemblyLists
{
    public class AssemblyListManager : IAssemblyListManager
    {
        private const string DefaultListName = "-- Default --";

        private readonly IList<string> listNames = new List<string>();

        public AssemblyListManager(IDictionary<string, IList<string>> assemblyLists, string selected)
        {
            AssemblyLists = CloneDictionary(assemblyLists);

            if (AssemblyLists.Count == 0)
                AssemblyLists.Add(DefaultListName, new List<string>());

            listNames = (from list in AssemblyLists
                         select list.Key).ToList();

            if (!listNames.Contains(selected))
                selected = DefaultListName;

            CurrentListName = selected;
        }

        private static Dictionary<string, IList<string>> CloneDictionary(IDictionary<string, IList<string>> dictionary)
        {
            return dictionary.ToDictionary(k => k.Key, v => v.Value);
        }

        public IDictionary<string, IList<string>> AssemblyLists { get; private set; }
        public IEnumerable<string> ListNames
        {
            get { return listNames; }
        }

        public string CurrentListName { get; set; }

        public bool Add(string listName)
        {
            if (!AssemblyLists.ContainsKey(listName))
            {
                listNames.Add(listName);
                AssemblyLists.Add(listName, new List<string>());
                return true;
            }

            return false;
        }

        public void Remove(string listName)
        {
            listNames.Remove(listName);
            AssemblyLists.Remove(listName);
        }

        public void Rename(string oldName, string newName)
        {
            var lists = AssemblyLists[oldName];
            AssemblyLists.Remove(oldName);
            AssemblyLists[newName] = lists;

            // This is why we maintain a separate list of names - so that the order doesn't change when we rename
            var indexOf = listNames.IndexOf(oldName);
            listNames[indexOf] = newName;
        }
    }
}