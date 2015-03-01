using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using CEWSP_v2.Backend;

namespace CEWSP_v2.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewProject.xaml
    /// </summary>
    public partial class CreateNewProject : Window
    {
        public static int MyProperty { get; set; }

        public CreateNewProject()
        {
            InitializeComponent();

            CreateToolTips();

            ceRootTextBox.Width -= 35;
            var b = new Rectangle() { Fill = Brushes.Aquamarine, Width = 30 };
            ceRootStackPanel.Children.Insert(1, b);
        }

        private void CreateToolTips()
        {
            projectImage.ToolTip = Utillities.ConstructToolTip(Properties.CreateNewProject.TipImage,
                                                               Properties.CreateNewProject.TipImageClickToBrowse);
        }
    }
}
