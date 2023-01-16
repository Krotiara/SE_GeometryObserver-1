using Autodesk.Revit.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace AppNamespace
{
    public class App : IExternalApplication
    {
        string AR_RIBBON_TAB = SE_GeometryObserver.Properties.Settings.Default.AR_RIBBON_TAB;
        string AR_RIBBON_PANEL = SE_GeometryObserver.Properties.Settings.Default.AR_RIBBON_PANEL;
        string AR_BUTTON_NAME = SE_GeometryObserver.Properties.Settings.Default.AR_BUTTON_NAME;
        string AR_BUTTON_TEXT = SE_GeometryObserver.Properties.Settings.Default.AR_BUTTON_TEXT;
        string AR_BUTTON_TOOLTIP = SE_GeometryObserver.Properties.Settings.Default.AR_BUTTON_TOOLTIP;
#warning в Assembly.GetEntryAssembly().GetName().Name ниже баг, нужно имя проекта ставить туда.
        string AR_BUTTON_BITMAPURI = $@"pack://application:,,,/SE_GeometryObserver;component/resources/icon.png";



        public Result OnStartup(UIControlledApplication a)
        {
            InitLogger();
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);

                try
                {
                    a.CreateRibbonTab(AR_RIBBON_TAB);
                }
                catch { }

                RibbonPanel ribbonPanel = GetRibbonPanel(AR_RIBBON_TAB, AR_RIBBON_PANEL, a);

                PushButtonData buttonData = new PushButtonData(
                    AR_BUTTON_NAME,
                    AR_BUTTON_TEXT,
                    Assembly.GetExecutingAssembly().Location,
                    typeof(AppNamespace.Command).FullName);
                PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
                pushButton.ToolTip = AR_BUTTON_TOOLTIP;
                BitmapImage largeImage = new BitmapImage(new Uri(AR_BUTTON_BITMAPURI));
                pushButton.LargeImage = largeImage;
                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                logger?.Error(ex);
                throw;
            }
        }

        public RibbonPanel GetRibbonPanel(string tab, string panel, UIControlledApplication a)
        {
            //Ищем панель с указанным именем на вкладке
            List<RibbonPanel> ribbonPanels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel ribbon in ribbonPanels)
            {
                if (ribbon.Name == panel)
                {
                    return ribbon;
                }
            }
            return a.CreateRibbonPanel(tab, panel);
        }


        static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            //https://stackoverflow.com/questions/1373100/how-to-add-folder-to-assembly-search-path-at-runtime-in-net
            //Необходимо для загрузки nuget пакетов
            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(App));
        static void InitLogger()
        {
            string rootLocaltion = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configPath = Path.Combine(rootLocaltion, "log4net.config");
            string pathToReplace = "<file value=\"C:\\TestLog\\TestLog.log\"/>";
            string path = $"<file value=\"{Path.Combine(rootLocaltion, $"{nameof(App)}.log")}\"/>";
            string data = File.ReadAllText(configPath).Replace(pathToReplace, path);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            Stream s = new MemoryStream(bytes);
            log4net.Config.XmlConfigurator.Configure(s);
            s.Close(); // Configure не закрывает Stream.
            logger.Info("        =============  Started Logging  =============        ");
        }



        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
