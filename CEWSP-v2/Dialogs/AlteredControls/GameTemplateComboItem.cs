using CEWSP_v2.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CEWSP_v2.Dialogs.AlteredControls
{
    class GameTemplateComboItem : ComboBoxItem
    {
        public GameTemplate AssociatedGameTemplate { get; set; }

    }
}
