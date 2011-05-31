using System.Collections.Generic;

namespace CitizenMatt.DotPeek.AssemblyLists
{
    public interface IAssemblyListManager
    {
        IEnumerable<string> ListNames { get; }
        string CurrentListName { get; set; }
        bool Add(string listName);
        void Remove(string listName);
        void Rename(string oldName, string newName);
    }
}