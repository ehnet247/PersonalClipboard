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
                this.Close();
            };
            autoCloseTimer.Start();
            ContextMenuStrip ms = new ContextMenuStrip();
            int maxCharCount = 0;
            
            if (_dataManager!.ClipboardData!.Phrases != null)
            {
                foreach (var phrase in _dataManager.ClipboardData!.Phrases)
                {
                    if (phrase.Name.Length > maxCharCount)
                        maxCharCount = phrase.Name.Length;
                }
                foreach (var phrase in _dataManager.ClipboardData!.Phrases)
                {
                    string buttonText = phrase.Name;
                    while (buttonText.Length < maxCharCount)
                    {
                        buttonText += "_";
                    }
                    ToolStripButton phraseButton = new ToolStripButton(buttonText, null, null, "Phrase");
                    phraseButton.Width = 100;
                    phraseButton.AutoSize = false;
                    if (phrase.IsSecret)
                        phraseButton.BackColor = System.Drawing.Color.Orange;
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
            configButton.Width = 100;
            configButton.AutoSize = false;
            while (configButton.Text.Length < maxCharCount)
            {
                configButton.Text += "_";
            }
            configButton.Width = 100;
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
            ToolStripButton closeButton = new ToolStripButton("Close", null, null, "Close");
            closeButton.Width = 100;
            closeButton.AutoSize = false;
            closeButton.BackColor = System.Drawing.Color.OrangeRed;
            closeButton.Click += (s, ev) => this.Close();
            while (closeButton.Text.Length < maxCharCount)
            {
                closeButton.Text += "_";
            }
            ms.Items.Add(configButton);
            ms.Items.Add(closeButton);
            ms.Dock = DockStyle.Fill;
            this.ContextMenuStrip = ms;
            this.ContextMenuStrip.Show(this,  new System.Drawing.Point(0, 0));
            //this.ContextMenuStrip.Width = 100;
        }
    }
}
