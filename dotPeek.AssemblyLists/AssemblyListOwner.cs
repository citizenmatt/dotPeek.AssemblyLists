using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.Application.Configuration;
using JetBrains.DataFlow;
using JetBrains.DotPeek.AssemblyExplorer;
using JetBrains.ProjectModel;
using JetBrains.Util;

namespace CitizenMatt.DotPeek.AssemblyLists
{
    [SolutionComponent]
    public class AssemblyListOwner : IXmlExternalizable
    {
        private readonly AssemblyExplorerManager assemblyExplorerManager;
        private AssemblyListManager manager = new AssemblyListManager(new Dictionary<string, IList<string>>(), null);

        public AssemblyListOwner(Lifetime lifetime, AssemblyExplorerManager assemblyExplorerManager, ShellSettingsComponent shellSettings)
        {
            this.assemblyExplorerManager = assemblyExplorerManager;
            shellSettings.LoadSettings(lifetime, this, XmlExternalizationScope.WorkspaceSettings, "AssemblyLists");
        }

        public void ReadFromXml(XmlElement element)
        {
            if (element == null)
                return;

            var currentList = element.ReadAttribute("current");

            var assemblyListGroups = from assemblyList in element.SelectElements("AssemblyList")
                                     let name = assemblyList.ReadAttribute("name")
                                     from assemblyFile in assemblyList.SelectElements("AssemblyFile")
                                     group XmlUtil.ReadLeafElementValue(assemblyFile) by name;

            var assemblyLists = assemblyListGroups.ToDictionary(k => k.Key, v => (IList<string>)v.ToList());

            manager = new AssemblyListManager(assemblyLists, currentList);
        }

        public void WriteToXml(XmlElement element)
        {
            RefreshCurrentAssemblyList();

            // TODO: How do we refresh the update the current list before saving
            if (!string.IsNullOrEmpty(manager.CurrentListName))
                element.SetAttribute("current", manager.CurrentListName);

            foreach (var assemblyListName in manager.AssemblyLists.Keys)
            {
                var listElement = element.CreateElement("AssemblyList");
                listElement.SetAttribute("name", assemblyListName);
                foreach (var path in manager.AssemblyLists[assemblyListName])
                {
                    listElement.CreateLeafElementWithValue("AssemblyFile", path);
                }
            }
        }

        public AssemblyListManager GetAssemblyListsForEditing()
        {
            RefreshCurrentAssemblyList();
            return new AssemblyListManager(manager.AssemblyLists, manager.CurrentListName);
        }

        public void SetAssemblyLists(AssemblyListManager newAssemblyListManager)
        {
            var currentlyDisplayedList = manager.CurrentListName;
            manager = newAssemblyListManager;
            if (manager.CurrentListName != currentlyDisplayedList)
            {
                DisplayAssemblyList(manager.AssemblyLists[manager.CurrentListName]);
            }
        }

        private void DisplayAssemblyList(IList<string> assemblyList)
        {
            assemblyExplorerManager.ResetVisibleAssemblies();

            if (assemblyList.Count == 0)
                assemblyList.AddRange(GetVisibleAssemblyList());
            else
            {
                ClearVisibleAssemblies();

                var paths = from path in assemblyList
                            select new FileSystemPath(path);

                assemblyExplorerManager.AddUserVisibleAssembly(paths.ToArray());
            }
        }

        private void ClearVisibleAssemblies()
        {
            var assemblyFiles = from assembly in assemblyExplorerManager.UserVisibleAssemblies
                                from file in assembly.GetFiles()
                                select file;

            foreach (var assemblyFile in assemblyFiles.Distinct(x => x.Location.FullPath))
            {
                assemblyExplorerManager.RemoveUserVisibleAssembly(assemblyFile);
            }
        }

        private void RefreshCurrentAssemblyList()
        {
            manager.AssemblyLists[manager.CurrentListName] = GetVisibleAssemblyList();
        }

        private List<string> GetVisibleAssemblyList()
        {
            var files = from assembly in assemblyExplorerManager.UserVisibleAssemblies
                        from file in assembly.GetFiles()
                        select file.Location.FullPath;
            return files.Distinct(x => x).ToList();
        }
    }
}