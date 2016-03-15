using CEWSP_Backend.Backend;
using System.Windows;
using System.Windows.Controls;

namespace CEWSP_v2.CustomControls
{
    public enum ECommandLineType
    {
        eCT_Editor64,
        eCT_Editor32,
        eCT_Game32,
        eCT_Game64,
    }

    /// <summary>
    /// Interaction logic for CommandLineControl.xaml
    /// </summary>
    public partial class CommandLineControl : UserControl
    {
        public ECommandLineType CommandLineType
        {
            get { return (ECommandLineType)GetValue(CommandLineTypeProperty); }
            set { SetValue(CommandLineTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandLineType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandLineTypeProperty =
            DependencyProperty.Register("CommandLineType", typeof(ECommandLineType), typeof(CommandLineControl), new PropertyMetadata(ECommandLineType.eCT_Editor64));

        public CommandLineControl()
        {
            InitializeComponent();
        }

        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            string sCommandLine = textBox.Text;
            switch (CommandLineType)
            {
                case ECommandLineType.eCT_Editor64:
                    ApplicationBackend.Start64bitEditor(sCommandLine);
                    break;

                case ECommandLineType.eCT_Editor32:
                    ApplicationBackend.Start32bitEditor(sCommandLine);
                    break;

                case ECommandLineType.eCT_Game32:
                    ApplicationBackend.Start32bitGame(sCommandLine);
                    break;

                case ECommandLineType.eCT_Game64:
                    ApplicationBackend.Start64bitGame(sCommandLine);
                    break;

                default:
                    break;
            }
        }
    }
}