using ClipboardData;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClipboardConfigForm.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ConfigView : MyWindow, IConfigView
    {
        private readonly IClipboardDataManager? _dataManager;


        public ConfigView()
        {
            InitializeComponent();
        }

        public ConfigView(IClipboardDataManager dataManager)
        {
            _dataManager = dataManager;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWhenNotBusy = true;
        }
    }

}
