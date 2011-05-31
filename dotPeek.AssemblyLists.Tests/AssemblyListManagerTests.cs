using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace CitizenMatt.DotPeek.AssemblyLists.Tests
{
    public class AssemblyListManagerTests
    {
        private static AssemblyListManager CreateAssemblyListManager()
        {
            return new AssemblyListManager(AssemblyListBuilder.New()
                                               .With("list1", "file1.dll")
                                               .With("list2", "file2.dll")
                                               .With("list3", "file1.dll", "file2.dll").Done(), "list1");
        }

        [Fact]
        public void CanSetCurrentlySelected()
        {
            var manager = CreateAssemblyListManager();
            manager.CurrentListName = "list2";

            Assert.Equal("list2", manager.CurrentListName);
        }

        [Fact]
        public void ListNamesExposesNamesOfAllLists()
        {
            var manager = CreateAssemblyListManager();
            Assert.Equal(new List<string> { "list1", "list2", "list3" }, manager.ListNames);
        }

        [Fact]
        public void CallingAddWillAddNameToListNames()
        {
            var manager = CreateAssemblyListManager();
            manager.Add("list4");
            Assert.Equal(new List<string> { "list1", "list2", "list3", "list4" }, manager.ListNames);
        }

        [Fact]
        public void CallingAddWillAddEmptyList()
        {
            var manager = CreateAssemblyListManager();
            manager.Add("list4");

            Assert.NotNull(manager.AssemblyLists["list4"]);
            Assert.IsAssignableFrom<IList<string>>(manager.AssemblyLists["list4"]);
            Assert.Equal(0, manager.AssemblyLists["list4"].Count);
        }

        [Fact]
        public void CallingAddWithExistingValueReturnsFalseAndDoesNotAddAgain()
        {
            var manager = CreateAssemblyListManager();
            Assert.False(manager.Add("list1"));
            Assert.True(manager.ListNames.Where(x => x == "list1").Count() == 1);
            Assert.True(manager.AssemblyLists.Where(x => x.Key == "list1").Count() == 1);
        }

        [Fact]
        public void CallingRemoveWillRemoveNameFromListNames()
        {
            var manager = CreateAssemblyListManager();
            manager.Remove("list1");
            Assert.Equal(new List<string> {"list2", "list3"}, manager.ListNames);
        }

        [Fact]
        public void CallingRemoveWillRemoveFromAssemblyLists()
        {
            var manager = CreateAssemblyListManager();
            manager.Remove("list1");
            Assert.Equal(new List<string> { "list2", "list3" }, manager.AssemblyLists.Select(x => x.Key).ToList());
        }

        [Fact]
        public void CallingRenameWillRemoveOldNameFromLists()
        {
            var manager = CreateAssemblyListManager();
            manager.Rename("list2", "newList");
            Assert.DoesNotContain("list2", manager.ListNames);
            //Assert.Equal(new List<string> {"list1", "newList", "list3"}, manager.ListNames);
        }

        [Fact]
        public void CallingRenameWillAddNewNameToLists()
        {
            var manager = CreateAssemblyListManager();
            manager.Rename("list2", "newList");
            Assert.Contains("newList", manager.ListNames);
        }

        [Fact]
        public void CallingRenameWillMaintainOrder()
        {
            var manager = CreateAssemblyListManager();
            manager.Rename("list2", "newList");
            Assert.Equal(new List<string> {"list1", "newList", "list3"}, manager.ListNames);
        }

        [Fact]
        public void CallingRenameWillMaintainExistingAssemblyList()
        {
            var manager = CreateAssemblyListManager();
            var existingList = manager.AssemblyLists["list2"];

            manager.Rename("list2", "newList");

            Assert.Same(existingList, manager.AssemblyLists["newList"]);
        }
    }
}
