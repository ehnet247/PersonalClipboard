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
        private System.Windows.Forms.Timer? _autoCloseTimer;
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
            _autoCloseTimer = new System.Windows.Forms.Timer();
            _autoCloseTimer.Interval = 30000; // 30 seconds
            _autoCloseTimer.Tick += (s, ev) =>
            {
                _autoCloseTimer.Dispose();
                this.Close();
            };
            _autoCloseTimer.Start();
            ContextMenuStrip ms = new ContextMenuStrip();
            
            if (_dataManager!.ClipboardData!.Phrases != null)
            {
                foreach (var phrase in _dataManager.ClipboardData!.Phrases)
                {
                    string buttonText = phrase.Name;
                    ToolStripButton phraseButton = new ToolStripButton(buttonText, null, null, "Phrase");
                    phraseButton.TextAlign = ContentAlignment.BottomLeft;
                    phraseButton.Width = 100;
                    phraseButton.Padding = Padding.Empty;
                    phraseButton.AutoSize = false;
                    phraseButton.ImageAlign = ContentAlignment.BottomLeft;
                    if (phrase.IsSecret)
                        phraseButton.BackColor = System.Drawing.Color.Yellow;
                    else
                        phraseButton.BackColor = System.Drawing.Color.DeepSkyBlue;
                    phraseButton.Click += (s, ev) =>
                    {
                        // Copy the phrase content to the clipboard
                        System.Windows.Forms.Clipboard.SetText(phrase.Content);
                        // Close the form after copying
                        this.Close();
                    };
                    ms.Items.Add(phraseButton);
                }
            }
            ToolStripButton configButton = new ToolStripButton("Config", null, null, "Config");
            configButton.TextAlign = ContentAlignment.BottomLeft;
            configButton.Width = 100;
            configButton.Padding = Padding.Empty;
            configButton.AutoSize = false;
            configButton.ImageAlign = ContentAlignment.BottomLeft;
            configButton.BackColor = System.Drawing.Color.LightGray;
            configButton.Width = 100;
            configButton.Click += (s, ev) =>
            {
                var configView = Program.ServiceProvider.GetRequiredService<IConfigView>();
                var configViewModel = Program.ServiceProvider.GetRequiredService<IConfigViewModel>();
                var configWindow = configView as Window;
                if (configWindow != null)
                {
                    if (configViewModel == null)
                        throw new InvalidOperationException("ConfigViewModel is not registered in the service provider.");
                    configWindow.DataContext = configViewModel;
                    // Stop the autoclose timer because it could take time to enter new phrases in the config window
                    if (_autoCloseTimer != null)
                    {
                        _autoCloseTimer.Stop();
                    }
                    configWindow.ShowDialog();
                }
                System.Windows.Forms.Application.Exit();
            };
            ToolStripButton closeButton = new ToolStripButton("Close", null, null, "Close");
            closeButton.TextAlign = ContentAlignment.BottomLeft;
            closeButton.Width = 100;
            closeButton.Padding = Padding.Empty;
            closeButton.AutoSize = false;
            closeButton.ImageAlign = ContentAlignment.BottomLeft;
            closeButton.BackColor = System.Drawing.Color.OrangeRed;
            closeButton.Click += (s, ev) => this.Close();
            ms.Items.Add(configButton);
            ms.Items.Add(closeButton);
            ms.Dock = DockStyle.Fill;
            this.ContextMenuStrip = ms;
            this.ContextMenuStrip.Show(this,  new System.Drawing.Point(0, 0));
            //this.ContextMenuStrip.Width = 100;
        }
    }
}
