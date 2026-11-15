using ClipboardConfigForm;
using ClipboardConfigForm.ViewModels;
using ClipboardConfigForm.Views;
using ClipboardData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WinFormsPersonalClipboard
{
    internal static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;
            //Application.Run(new PersonalClipboardForm());
            var rootForm = ServiceProvider.GetRequiredService<IRootForm>() as Form;
            if (rootForm != null)
                Application.Run(rootForm);
        }
        public static IServiceProvider? ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddTransient<IConfigView, ConfigView>();
                    services.AddTransient<IClipboardDataManager, ClipboardDataManager>();
                    services.AddTransient<IRootForm, PersonalClipboardForm>();
                    services.AddTransient<IConfigViewModel, ConfigViewModel>();
                });
        }
    }
}