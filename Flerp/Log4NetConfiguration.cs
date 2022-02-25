using Flerp.Client.Helpers;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace Flerp
{
    public static class Log4NetConfiguration
    {
        public static void Configure()
        {
            var logFileLayout = new PatternLayout("%-15date{HH:mm:ss} %-5[%thread] %-5level : %message%newline");
            var logFileAppender = new RollingFileAppender
            {
                File = "flerp.log",
                Layout = logFileLayout
            };
            logFileLayout.ActivateOptions();
            logFileAppender.ActivateOptions();


            var consoleLayout = new PatternLayout("%-15date{HH:mm:ss} %-5level : %message%newline");
            var consoleAppender = new ConsoleAppender 
            {
                Layout = consoleLayout
            };
            consoleLayout.ActivateOptions();
            consoleAppender.ActivateOptions();

            var guiLayout = new PatternLayout("%date{HH:mm:ss} %-5level : %message%newline");
            var guiAppender = new TextBoxAppender
            {
                Layout = guiLayout,
                TextBoxName = "memoEdit1",
                FormName = "MainView",
                Threshold = Level.Info
            };
            guiLayout.ActivateOptions();
            guiAppender.ActivateOptions();

            IAppender[] list = { logFileAppender, consoleAppender, guiAppender };
            BasicConfigurator.Configure(list);           
        }
    }
}