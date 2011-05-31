using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;

namespace CitizenMatt.DotPeek.AssemblyLists
{
    [ActionHandler("CitizenMatt.AssemblyLists")]
    public class AssemblyListActionHandler : IActionHandler
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var solution = context.GetData(JetBrains.ProjectModel.DataContext.DataConstants.SOLUTION);
            var assemblyListOwner = solution.GetComponent<AssemblyListOwner>();

            var assemblyListManager = assemblyListOwner.GetAssemblyListsForEditing();
            var dialog = new AssemblyListForm(assemblyListManager);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                assemblyListOwner.SetAssemblyLists(assemblyListManager);
            }
        }
    }
}
