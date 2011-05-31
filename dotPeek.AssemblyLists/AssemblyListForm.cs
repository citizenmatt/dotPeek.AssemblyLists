using System;
using System.Windows.Forms;
using System.Linq;

namespace CitizenMatt.DotPeek.AssemblyLists
{
    public partial class AssemblyListForm : Form
    {
        private readonly IAssemblyListManager assemblyListManager;

        public AssemblyListForm(IAssemblyListManager assemblyListManager)
        {
            this.assemblyListManager = assemblyListManager;

            InitializeComponent();

            AddLists();
            listView.AfterLabelEdit += listView_AfterLabelEdit;
        }

        private void AddLists()
        {
            listView.Items.Clear();
            var items = from listName in assemblyListManager.ListNames
                        select new ListViewItem(listName) { Name = listName };

            listView.Items.AddRange(items.ToArray());
            var foundItems = listView.Items.Find(assemblyListManager.CurrentListName, false);
            if (foundItems.Length > 0)
                foundItems[0].Selected = true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var newName = GetNewListName();
            assemblyListManager.Add(newName);

            var listViewItem = new ListViewItem(newName);
            listView.Items.Add(listViewItem);
            listViewItem.BeginEdit();
        }

        private string GetNewListName()
        {
            string newName;
            int counter = 0;
            do
            {
                newName = string.Format("New List {0}", ++counter);
            } while (assemblyListManager.ListNames.Contains(newName));

            return newName;
        }

        void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                assemblyListManager.Rename(listView.Items[e.Item].Text, e.Label);
                assemblyListManager.CurrentListName = e.Label;
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            // TODO: Confirmation? Probably not. You have to confirm the dialog before it's removed
            var listName = listView.SelectedItems[0].Text;
            assemblyListManager.Remove(listName);
            listView.Items.Remove(listView.SelectedItems[0]);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: We really shouldn't have explicit knowledge that index 0 can't be deleted
            removeButton.Enabled = listView.SelectedIndices.Count > 0 && listView.SelectedIndices[0] != 0;
            selectButton.Enabled = listView.SelectedIndices.Count > 0;

            if (listView.SelectedIndices.Count > 0)
            {
                assemblyListManager.CurrentListName = listView.Items[listView.SelectedIndices[0]].Text;
            }
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
