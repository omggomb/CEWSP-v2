using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEWSP_v2.CustomControls
{
    class ExplorerItemFactory : ExplorerTreeView.TreeItemFactory
    {

        public ExplorerTreeView.CustomTreeItem CreateCustomTreeItemInstance()
        {
            return new ExplorerTreeView.CustomTreeItem();
        }

        public System.Windows.Controls.Image CreateFolderIconImage(ExplorerTreeView.CustomTreeItem itemThatIsUsed)
        {
            return null;
        }
    }
}
