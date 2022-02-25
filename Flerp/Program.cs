using Autofac;
using Flerp.Data;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Flerp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log4NetConfiguration.Configure();
            var container = AutofacConfiguration.Configure();
            var dbController = container.Resolve<IDatabaseController>();
            var apController = container.Resolve<IController>();
            
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                SetDefaultAppSettings();
                
                dbController.Start();               
                apController.Start(container);
                Application.Run((Form)Controller.MainView);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                dbController.Stop();
            }
        }

        static void SetDefaultAppSettings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var modified = false;

            if (Debugger.IsAttached) config.AppSettings.Settings.Clear();
            if (!config.AppSettings.Settings.AllKeys.Contains("DbServerPath")) config.AppSettings.Settings.Add("DbServerPath", @"\MongoDB\Server\3.0\bin\mongod.exe"); modified = true;
            //if (!config.AppSettings.Settings.AllKeys.Contains("DbServerArgs")) config.AppSettings.Settings.Add("DbServerArgs", @"--port {0} --dbpath ""{1}"" --smallfiles"); modified = true;
            if (!config.AppSettings.Settings.AllKeys.Contains("ConnectionString")) config.AppSettings.Settings.Add("ConnectionString", @"mongodb://localhost:{0}"); modified = true;
            if (!config.AppSettings.Settings.AllKeys.Contains("CloudServiceProvider")) config.AppSettings.Settings.Add("CloudServiceProvider", @"None"); modified = true;
            if (!config.AppSettings.Settings.AllKeys.Contains("CloudAccountName")) config.AppSettings.Settings.Add("CloudAccountName", @"None"); modified = true;
            if (!config.AppSettings.Settings.AllKeys.Contains("CloudAccountKey")) config.AppSettings.Settings.Add("CloudAccountKey", @"None"); modified = true;
            if (!config.AppSettings.Settings.AllKeys.Contains("EmailDownloadCap")) config.AppSettings.Settings.Add("EmailDownloadCap", @"10"); modified = true;

            if (modified)
            {
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
