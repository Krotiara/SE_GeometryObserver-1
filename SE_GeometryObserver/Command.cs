using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using log4net;
using SE_ElementsSelector.Models;
using SE_GeometryObserver.Interfaces;
using SE_GeometryObserver.Service.Service;
using SE_GeometryObserver.Views;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AppNamespace
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public IntPtr GetHandleWindow(string title)
        {
            return FindWindow(null, title);
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(Command));
        private static SelectionModel selectionModel;

        public Command()
        {

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


        static void InitLogger()
        {
            string rootLocaltion = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configPath = Path.Combine(rootLocaltion, "log4net.config");
            string pathToReplace = "<file value=\"C:\\TestLog\\TestLog.log\"/>";
            string path = $"<file value=\"{Path.Combine(rootLocaltion, $"{nameof(Command)}.log")}\"/>";
            string data = File.ReadAllText(configPath).Replace(pathToReplace, path);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            Stream s = new MemoryStream(bytes);
            log4net.Config.XmlConfigurator.Configure(s);
            s.Close(); // Configure не закрывает Stream.
            logger.Info("        =============  Started Logging  =============        ");
        }



        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromSameFolder);
                try
                {
                    IntPtr pluginWindow = GetHandleWindow(SE_GeometryObserver.Properties.Settings.Default.Title);
                    if (pluginWindow != IntPtr.Zero)
                    {
                        TaskDialog.Show("Info", "Приложение уже запущено.");
                        logger.Info("Попытка повторного открытия плагина при уже открытом плагине.");
                        return Result.Cancelled;

                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Ошибка проверки повторно открытого окна.", ex.Message);
                    logger.Error("Ошибка проверки повторно открытого окна.", ex);
                }

                InitLogger();
                RevitBridge.RevitBridge revitBridge = new RevitBridge.RevitBridge(commandData.Application);
                selectionModel = new SelectionModel(revitBridge);

                IGeometryComparer gC = new GeometryComparerByEquals(
                        new CalculateNearestGeomModelsService(),
                        new CompareGeometryElementsService());

                GeometryLoaderView view = new GeometryLoaderView(revitBridge, gC, selectionModel, logger);
                view.Show();
                //init main viewLogic


                return Result.Succeeded;
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", e.Message);
                logger.Error(e);
                logger.Logger.Repository.Shutdown(); //закрыть логгер.
                return Result.Failed;
            }
        }
    }
}
