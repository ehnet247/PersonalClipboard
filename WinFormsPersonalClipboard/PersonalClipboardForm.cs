using ClipboardConfigForm;
using ClipboardConfigForm.Views;
using ClipboardData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Devices;
using System.Windows;
using System.Xaml;

namespace WinFormsPersonalClipboard
{
    public partial class PersonalClipboardForm : Form, IRootForm
    {
        private readonly IClipboardDataManager? _dataManager;
        public PersonalClipboardForm(IClipboardDataManager dataManager)
        {
            _dataManager = dataManager;
            InitializeComponent();
        }
        public PersonalClipboardForm()
        {
            InitializeComponent();
        }

        private void PersonalClipboardForm_Load(object sender, EventArgs e)
        {
            this.Location = Cursor.Position;
            // Setup auto-close timer
            var autoCloseTimer = new System.Windows.Forms.Timer();
            autoCloseTimer.Interval = 60000; // 60 seconds
            autoCloseTimer.Tick += (s, ev) =>
            {
                autoCloseTimer.Dispose();
                //this.Close();
            };
            autoCloseTimer.Start();
            ContextMenuStrip ms = new ContextMenuStrip();
            ToolStripDropDownButton closeButton = new ToolStripDropDownButton("Close", null, null, "Close");
            closeButton.ShowDropDownArrow = false;
            closeButton.Click += (s, ev) => this.Close();
            ToolStripDropDownButton configButton = new ToolStripDropDownButton("Config", null, null, "Config");
            configButton.ShowDropDownArrow = false;
            configButton.Click += (s, ev) =>
            {
                //var configForm = new ConfigView();
                var configView = Program.ServiceProvider.GetRequiredService<IConfigView>();
                var configViewModel = Program.ServiceProvider.GetRequiredService<IConfigViewModel>();
                var configWindow = configView as Window;
                if (configWindow != null)
                {
                    if (configViewModel == null)
                        throw new InvalidOperationException("ConfigViewModel is not registered in the service provider.");
                    configWindow.DataContext = configViewModel;
                    configWindow.ShowDialog();
                }
                System.Windows.Forms.Application.Exit();
            };
            if (_dataManager.ClipboardData!.Phrases != null)
            {
                foreach (var phrase in _dataManager.ClipboardData!.Phrases)
                {
                    ToolStripDropDownButton phraseButton = new ToolStripDropDownButton(phrase.Name, null, null, "Close");
                }
            }
            foreach(var phrase in _dataManager.ClipboardData!.Phrases)
            {
                ToolStripDropDownButton phraseButton = new ToolStripDropDownButton(phrase.Name, null, null, "Phrase");
                phraseButton.ShowDropDownArrow = false;
                phraseButton.Click += (s, ev) =>
                {
                    // Copy the phrase content to the clipboard
                    System.Windows.Forms.Clipboard.SetText(phrase.Content);
                    // Close the form after copying
                    this.Close();
                };
                ms.Items.Add(phraseButton);
            }
            ms.Items.Add(configButton);
            ms.Items.Add(closeButton);
            ms.Dock = DockStyle.Fill;
            this.ContextMenuStrip = ms;
            this.ContextMenuStrip.Show(this,  new System.Drawing.Point(0, 0));
        }
    }
}
